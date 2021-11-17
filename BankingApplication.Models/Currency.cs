using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication.Models
{
    public class Currency
    {
        public Currency(string name, decimal exchangeRate)
        {
            CurrencyName = name;
            ExchangeRate = exchangeRate;
        }
        public string CurrencyName { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
