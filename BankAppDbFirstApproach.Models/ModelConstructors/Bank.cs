using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppDbFirstApproach.Models
{
    public partial class Bank
    {
        public Bank(string name, string branch, string ifsc)
        {
            this.bankname = name;
            this.bankId = $"{name.Substring(0, 3)}{DateTime.Now:yyyyMMddhhmmss}";
            this.branch = branch;
            this.ifsc = ifsc;
            this.selfRTGS = 0;
            this.selfIMPS = 5;
            this.otherRTGS = 2;
            this.otherIMPS = 6;
            this.balance = 0;
            this.defaultCurrencyName = "INR";
        }
    }
}
