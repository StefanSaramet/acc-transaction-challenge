namespace AccountsTransactionsChallenge.Models;

public class Transaction
{
    public string Iban { get; set; } = String.Empty;
    public int TransactionId { get; set; }
    public decimal Amount { get; set; }
    public TransactionCategories CategoryId { get; set; }
    public DateOnly TransactionDate { get; set; }
}

public enum TransactionCategories
{
    Food = 1,
    Entertainment = 2,
    Clothing = 3,
    Travel = 4,
    MedicalExpenses = 5
}