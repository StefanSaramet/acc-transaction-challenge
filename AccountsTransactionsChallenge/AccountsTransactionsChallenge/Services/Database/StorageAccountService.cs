using System.Collections.ObjectModel;
using AccountsTransactionsChallenge.Data.Accounts;
using AccountsTransactionsChallenge.Models;

namespace AccountsTransactionsChallenge.Services.Database;

public class StorageAccountService : IAccountService
{
    private readonly IAccountsRepository _accountsRepository;

    public StorageAccountService(IAccountsRepository accountsRepository)
    {
        _accountsRepository = accountsRepository ?? throw new ArgumentNullException(nameof(accountsRepository));
    }

    public IReadOnlyCollection<Account> GetAllAccounts()
    {
        return new ReadOnlyCollection<Account>(_accountsRepository.GetAllAccounts().ToList());
    }
}