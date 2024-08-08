using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;

namespace Wallet.Infrastructure;

public class WalletDbInitializer
{
    private readonly WalletContext _context;
    private readonly ILogger<WalletDbInitializer> _logger;

    public WalletDbInitializer(WalletContext context, ILogger<WalletDbInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public void MigrateDatabase()
    {
        _logger.LogInformation("Migrating database associated with WalletContext");

        try
        {
            //if the sql server container is not created on run docker compose this migration can't fail for network related exception.
            var retry = Policy.Handle<SqlException>().WaitAndRetry(new TimeSpan[]
                { TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(8) });
            retry.Execute(() => _context.Database.Migrate());

            _logger.LogInformation("Migrated database associated with WalletContext");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while migrating the database used on WalletContext");
        }
    }
}