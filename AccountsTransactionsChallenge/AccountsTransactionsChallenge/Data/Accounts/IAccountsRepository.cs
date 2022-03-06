using AccountsTransactionsChallenge.Models;

namespace AccountsTransactionsChallenge.Data.Accounts;

public interface IAccountsRepository
{
    IReadOnlyCollection<Account> GetAllAccounts();
    Account GetAccountBy(string id);
}