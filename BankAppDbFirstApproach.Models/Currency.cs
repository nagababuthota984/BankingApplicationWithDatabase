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
    
    public partial class Currency
    {
        public string Id { get; set; }
        public string name { get; set; }
        public decimal exchangeRate { get; set; }
        public string bankId { get; set; }
    
        public virtual Bank Bank { get; set; }
    }
}