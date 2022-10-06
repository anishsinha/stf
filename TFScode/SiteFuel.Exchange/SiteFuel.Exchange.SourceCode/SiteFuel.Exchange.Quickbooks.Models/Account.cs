using System;
using System.Xml.Serialization;
using SiteFuel.Exchange.Quickbooks.SharedEnums;

namespace SiteFuel.Exchange.Quickbooks.Models
{
    [Serializable]
    public class AccountAdd
    {
        public AccountAdd()
        {
            AccountType = QbAccountType.Income;
        }

        public string Name { get; set; }

        /// <summary>
        /// AccountType may have one of the following values: AccountsPayable, AccountsReceivable, Bank, 
        /// CostOfGoodsSold, CreditCard, Equity, Expense, FixedAsset, Income, LongTermLiability, NonPosting, 
        /// OtherAsset, OtherCurrentAsset, OtherCurrentLiability, OtherExpense, OtherIncome
        /// </summary>
        public QbAccountType AccountType { get; set; }

        public string Desc { get; set; }
    }

    [Serializable]
    public class AccountAddRq
    {
        public AccountAddRq()
        {
            AccountAdd = new AccountAdd();
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public AccountAdd AccountAdd { get; set; }
    }

    [Serializable]
    public class AccountReturn
    {
        public int ListID { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime TimeModified { get; set; }

        public string EditSequence { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public bool IsActive { get; set; }

        public int Sublevel { get; set; }

        /// <summary>
        /// AccountType may have one of the following values: AccountsPayable, AccountsReceivable, Bank, 
        /// CostOfGoodsSold, CreditCard, Equity, Expense, FixedAsset, Income, LongTermLiability, NonPosting, 
        /// OtherAsset, OtherCurrentAsset, OtherCurrentLiability, OtherExpense, OtherIncome
        /// </summary>
        public QbAccountType AccountType { get; set; }

        public string Desc { get; set; }
    }

    [Serializable]
    public class AccountAddRs
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("AccountRet")]
        public AccountReturn AccountReturn { get; set; }
    }

    [Serializable]
    public class AccountMod
    {
        public int ListID { get; set; }

        public string EditSequence { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// AccountType may have one of the following values: AccountsPayable, AccountsReceivable, Bank, 
        /// CostOfGoodsSold, CreditCard, Equity, Expense, FixedAsset, Income, LongTermLiability, NonPosting, 
        /// OtherAsset, OtherCurrentAsset, OtherCurrentLiability, OtherExpense, OtherIncome
        /// </summary>
        public QbAccountType AccountType { get; set; }

        public string Desc { get; set; }
    }

    [Serializable]
    public class AccountModRq
    {
        public AccountModRq()
        {
            AccountMod = new AccountMod();
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public AccountMod AccountMod { get; set; }
    }

    [Serializable]
    public class AccountModRs
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlAttribute("statusCode")]
        public int StatusCode { get; set; }

        [XmlAttribute("statusSeverity")]
        public string StatusSeverity { get; set; }

        [XmlAttribute("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlElement("AccountRet")]
        public AccountReturn AccountReturn { get; set; }
    }

    [Serializable]
    public class AccountQueryRq
    {
        public AccountQueryRq()
        {
            ActiveStatus = ActiveStatus.ActiveOnly;
            RequestId = "{:RequestId:}";
        }

        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        public int ListID { get; set; }

        public string FullName { get; set; }

        /// <summary>
        /// ActiveStatus may have one of the following values: ActiveOnly [DEFAULT], InactiveOnly, All
        /// </summary>
        public ActiveStatus ActiveStatus { get; set; }

        public string FromModifiedDate { get; set; }

        public string ToModifiedDate { get; set; }

        /// <summary>
        /// MatchCriterion may have one of the following values: StartsWith, Contains, EndsWith
        /// </summary>
        public NameFilter NameFilter { get; set; }

        /// <summary>
        /// AccountType may have one of the following values: AccountsPayable, AccountsReceivable, Bank, 
        /// CostOfGoodsSold, CreditCard, Equity, Expense, FixedAsset, Income, LongTermLiability, NonPosting, 
        /// OtherAsset, OtherCurrentAsset, OtherCurrentLiability, OtherExpense, OtherIncome
        /// </summary>
        public QbAccountType AccountType { get; set; }
    }

    [Serializable]
    public class AccountQueryRs
    {
        [XmlAttribute("requestID")]
        public string RequestId { get; set; }

        [XmlElement("AccountRet")]
        public AccountReturn AccountReturn { get; set; }
    }
}
