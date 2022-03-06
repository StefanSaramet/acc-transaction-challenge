using System.Collections.ObjectModel;
using AccountsTransactionsChallenge.Models;

namespace AccountsTransactionsChallenge.Services.Remote;

public class HttpClientStorageAccountService : IAccountService
{
    public HttpClientStorageAccountService()
    {
        
    }

    public IReadOnlyCollection<Account> GetAllAccounts()
    {
        return new ReadOnlyCollection<Account>(null!);
    }
}