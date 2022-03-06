namespace AccountsTransactionsChallenge.DTOs;

public class TransactionReadDto
{
    public string Iban { get; set; } = String.Empty;
    public int TransactionId { get; set; }
    public decimal Amount { get; set; }
    public int CategoryId { get; set; }
    public DateOnly TransactionDate { get; set; }
}