using AccountsTransactionsChallenge.Models;

namespace AccountsTransactionsChallenge.Services;

public interface IAccountService
{
    IReadOnlyCollection<Account> GetAllAccounts();
}