using AccountsTransactionsChallenge.Services.Remote;
using Microsoft.AspNetCore.Mvc;

namespace AccountsTransactionsChallenge.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    [HttpGet]
    public async Task<string> GetToken()
    {
        return await _tokenService.GetToken();
    }
}