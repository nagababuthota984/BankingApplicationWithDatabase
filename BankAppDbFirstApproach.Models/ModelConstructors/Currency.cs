using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppDbFirstApproach.Models
{
    public partial class Currency
    {
        public Currency()
        {

        }
        public Currency(string name, decimal exchangeRate, string bankId)
        {
            this.Id = bankId + name;
            this.name = name;
            this.exchangeRate = exchangeRate;
            this.bankId = bankId;
        }
    }
}
