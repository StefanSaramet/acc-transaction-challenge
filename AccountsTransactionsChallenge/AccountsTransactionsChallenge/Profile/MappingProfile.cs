using AccountsTransactionsChallenge.DTOs;
using AccountsTransactionsChallenge.Models;

namespace AccountsTransactionsChallenge.Profile;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<Account, AccountReadDto>();
        CreateMap<Transaction, TransactionReadDto>();
    }
}