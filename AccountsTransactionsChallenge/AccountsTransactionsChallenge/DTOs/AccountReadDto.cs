namespace AccountsTransactionsChallenge.DTOs;

public class AccountReadDto
{
    public string ResourceId { get; set; } = String.Empty;
    public string Product { get; set; } = String.Empty;
    public string Iban { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public string Currency { get; set; } = String.Empty;
}