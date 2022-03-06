using AccountsTransactionsChallenge.Models;

namespace AccountsTransactionsChallenge.Data.Transactions;

public interface ITransactionRepository
{
    IReadOnlyCollection<Transaction> GetAllTransactions();
    Transaction GetTransactionBy(int id);
}