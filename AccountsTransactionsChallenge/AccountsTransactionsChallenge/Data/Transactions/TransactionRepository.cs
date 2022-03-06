using System.Collections.ObjectModel;
using AccountsTransactionsChallenge.Models;

namespace AccountsTransactionsChallenge.Data.Transactions;

public class TransactionRepository : ITransactionRepository
{
    private const string TransactionsKey = "Transactions";

    private readonly AppDbContext _dbContext;

    public TransactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IReadOnlyCollection<Transaction> GetAllTransactions()
    {
        var transactions = _dbContext.Database.GetCollection<Transaction>(TransactionsKey);

        return new ReadOnlyCollection<Transaction>(transactions.FindAll().ToList());
    }

    public Transaction GetTransactionBy(int id)
    {
        var transactions = _dbContext.Database.GetCollection<Transaction>(TransactionsKey);

        return transactions.FindById(id);
    }
}