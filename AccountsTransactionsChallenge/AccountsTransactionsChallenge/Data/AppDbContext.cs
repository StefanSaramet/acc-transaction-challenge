using AccountsTransactionsChallenge.Config;
using LiteDB;
using Microsoft.Extensions.Options;

namespace AccountsTransactionsChallenge.Data;

public class AppDbContext
{
    public LiteDatabase Database { get; }

    public AppDbContext(IOptions<DbOptions> options)
    {
        Database = new LiteDatabase(options.Value.DatabaseName);
    }
}