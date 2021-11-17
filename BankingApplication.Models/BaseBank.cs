using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication.Models
{
    public  class BaseBank
    {
        public string BankId { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string Ifsc { get; set; }
    }
    
}
