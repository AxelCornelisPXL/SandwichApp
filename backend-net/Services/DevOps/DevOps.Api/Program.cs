using Api.Filters;
using Api.MassTransit;
using Api.Swagger;
using DevOps.AppLogic;
using DevOps.AppLogic.Events;
using DevOps.Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(provider =>
    new ApplicationExceptionFilterAttribute(provider
        .GetRequiredService<ILogger<ApplicationExceptionFilterAttribute>>()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
string identityUrlExternal = builder.Configuration.GetValue<string>("Urls:IdentityUrlExternal");
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DevOps.Api", Version = "v1" });
    string securityScheme = "OpenID";
    var scopes = new Dictionary<string, string>
    {
        { "devops.read", "DevOps API - Read access" },
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
builder.Services.AddDbContext<DevOpsContext>(options =>
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
    x.AddConsumer<EmployeeHiredEventConsumer>();
    var rabbitMqSection = builder.Configuration.GetSection("EventBus:RabbitMQ");
    var rabbitMqSettings = new RabbitMqSettings();
    rabbitMqSection.Bind(rabbitMqSettings);
    x.UseRabbitMq(rabbitMqSettings);
});
builder.Services.AddScoped<DevOpsDbInitializer>();
builder.Services.AddScoped<IDeveloperRepository, DeveloperRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var readPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .RequireClaim("scope", "devops.read")
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
        string identityUrl = builder.Configuration.GetValue<string>("Urls:IdentityUrl");
        options.Authority = identityUrl;
        options.Audience = "devops";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false
        };
    });
var app = builder.Build();
var startUpScope = app.Services.CreateScope();
var initializer = startUpScope.ServiceProvider.GetRequiredService<DevOpsDbInitializer>();
initializer.MigrateDatabase();
initializer.SeedData();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevOps.Api v1");
        c.OAuthClientId("swagger.devops");
        c.OAuthUsePkce();
    });
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();