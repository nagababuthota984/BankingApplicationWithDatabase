using AutoMapper;
using BankAppDbFirstApproach.Data;
using BankAppDbFirstApproach.Models;
using System.Collections.Generic;

namespace BankAppDbFirstApproach.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountViewModel, Account>();
            CreateMap<Account, AccountViewModel>();
            CreateMap<BankViewModel, Bank>();
            CreateMap<Bank, BankViewModel>();
            CreateMap<EmployeeViewModel, Employee>();
            CreateMap<Employee, EmployeeViewModel>();
            CreateMap<CurrencyViewModel, Currency>();
            CreateMap<Currency, CurrencyViewModel>();
            CreateMap<CustomerViewModel, Customer>();
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<TransactionViewModel, Transaction>();
            CreateMap<Transaction, TransactionViewModel>();


        }
    }
}
