using AccountsTransactionsChallenge.DTOs;
using AccountsTransactionsChallenge.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AccountsTransactionsChallenge.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public AccountsController(IAccountService accountService, IMapper mapper)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public ActionResult<AccountListDto> GetAllAccounts()
    {
        var accounts = _accountService.GetAllAccounts();

        return Ok(new AccountListDto
        {
            Accounts = _mapper.Map<List<AccountReadDto>>(accounts)
        });
    }
}