//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BankAppDbFirstApproach.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transaction
    {
        public string transId { get; set; }
        public string accountId { get; set; }
        public string sendername { get; set; }
        public string receivername { get; set; }
        public System.DateTime transactionOn { get; set; }
        public decimal transactionAmount { get; set; }
        public decimal balance { get; set; }
        public int modeOfTransfer { get; set; }
        public string currency { get; set; }
        public int transactionType { get; set; }
        public string bankId { get; set; }
        public string otherPartyBankId { get; set; }
        public Nullable<bool> isBankTransaction { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Bank Bank { get; set; }
        public virtual Bank Bank1 { get; set; }
    }
}