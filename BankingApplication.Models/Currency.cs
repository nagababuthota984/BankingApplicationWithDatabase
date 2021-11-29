using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankingApplication.Models
{
    public class Currency
    {
        public Currency(string name, decimal exchangeRate,string bankId)
        {
            Id = bankId + name;
            Name = name;
            ExchangeRate = exchangeRate;
            BankId = bankId;
        }
        [Key]
        public string Id { get; set; } 
        public string Name { get; set; }
        public decimal ExchangeRate { get; set; }
        public string BankId { get; set; }
    }
}
