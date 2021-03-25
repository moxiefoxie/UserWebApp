//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UserWebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChartOfAccount
    {
        public int AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountDescription { get; set; }
        public string NormalSide { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public Nullable<decimal> InitialBalance { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public decimal Balance { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Orders { get; set; }
        public string Statements { get; set; }
        public string Comment { get; set; }
    }
}
