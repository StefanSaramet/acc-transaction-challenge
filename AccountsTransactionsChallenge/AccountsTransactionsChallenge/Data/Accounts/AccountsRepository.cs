using System.Collections.ObjectModel;
using AccountsTransactionsChallenge.Models;

namespace AccountsTransactionsChallenge.Data.Accounts;

public class AccountsRepository : IAccountsRepository
{
    private const string AccountsKey = "Accounts";

    private readonly AppDbContext _dbContext;

    public AccountsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IReadOnlyCollection<Account> GetAllAccounts()
    {
        var accounts = _dbContext.Database.GetCollection<Account>(AccountsKey);

        return new ReadOnlyCollection<Account>(accounts.FindAll().ToList());
    }

    public Account GetAccountBy(string id)
    {
        var accounts = _dbContext.Database.GetCollection<Account>(AccountsKey);

        return accounts.FindById(id);
    }
}