namespace AccountsTransactionsChallenge.Services.Remote;

public interface ITokenService
{
    public Task<string> GetToken();
}