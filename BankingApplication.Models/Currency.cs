using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication.Models
{
    public class Currency
    {
        public Currency(string name, decimal exchangeRate)
        {
            Name = name;
            ExchangeRate = exchangeRate;
        }
        public string Name { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
