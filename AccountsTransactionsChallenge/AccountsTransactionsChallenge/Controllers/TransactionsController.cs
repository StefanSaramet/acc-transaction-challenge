using AccountsTransactionsChallenge.Data.Transactions;
using AccountsTransactionsChallenge.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AccountsTransactionsChallenge.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public TransactionsController(ITransactionRepository transactionRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet("report")]
    public IActionResult GetTransactions()
    {
        var transactions = _transactionRepository.GetAllTransactions();

        return Ok(_mapper.Map<List<TransactionReadDto>>(transactions));
    }
}