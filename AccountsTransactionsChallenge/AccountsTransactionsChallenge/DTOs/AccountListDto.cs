namespace AccountsTransactionsChallenge.DTOs;

public class AccountListDto
{
    public IEnumerable<AccountReadDto> Accounts { get; set; } = new List<AccountReadDto>();
}