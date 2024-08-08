using Api.Filters;
using Api.MassTransit;
using Api.Swagger;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Wallet.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var identityUrlExternal = builder.Configuration.GetValue<string>("Urls:IdentityUrlExternal");
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wallet.Api", Version = "v1" });
    var securityScheme = "OpenID";
    var scopes = new Dictionary<string, string>
    {
        { "wallet.read", "Wallet API - Read access" },
        { "manage", "Write access" }
    };
    c.AddSecurityDefinition(securityScheme, new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{identityUrlExternal}/connect/authorize"),
                TokenUrl = new Uri($"{identityUrlExternal}/connect/token"),
                Scopes = scopes
            }
        }
    });
    c.OperationFilter<AlwaysAuthorizeOperationFilter>(securityScheme, scopes.Keys.ToArray());
});

var configuration = builder.Configuration;
builder.Services.AddDbContext<WalletContext>(options =>
{
    var connectionString = configuration["ConnectionString"];
    options.UseSqlServer(connectionString,
        sqlOptions => { sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null); });
#if DEBUG
    options.UseLoggerFactory(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddDebug()));
    options.EnableSensitiveDataLogging();
#endif
});
builder.Services.AddMassTransit(x =>
{
    // x.AddConsumer<EmployeeHiredEventConsumer>(); //create an event to subscribe too here
    var rabbitMqSection = builder.Configuration.GetSection("EventBus:RabbitMQ");
    var rabbitMqSettings = new RabbitMqSettings();
    rabbitMqSection.Bind(rabbitMqSettings);
    x.UseRabbitMq(rabbitMqSettings);
});
builder.Services.AddScoped<WalletDbInitializer>();
builder.Services.AddScoped<IWalletRepository, WalletDbRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var readPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .RequireClaim("scope", "wallet.read")
    .Build();
builder.Services.AddSingleton(provider =>
    new ApplicationExceptionFilterAttribute(provider
        .GetRequiredService<ILogger<ApplicationExceptionFilterAttribute>>()));
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<ApplicationExceptionFilterAttribute>();
    options.Filters.Add(new AuthorizeFilter(readPolicy));
});

var writePolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .RequireClaim("scope", "manage")
    .Build();
builder.Services.AddAuthorization(options => { options.AddPolicy("write", writePolicy); });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var identityUrl = builder.Configuration.GetValue<string>("Urls:IdentityUrl");
        options.Authority = identityUrl;
        options.Audience = "wallet";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false
        };
    });

var app = builder.Build();
var startUpScope = app.Services.CreateScope();
var initializer = startUpScope.ServiceProvider.GetRequiredService<WalletDbInitializer>();
initializer.MigrateDatabase();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wallet.Api v1");
        c.OAuthClientId("swagger.wallet");
        c.OAuthUsePkce();
    });
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();