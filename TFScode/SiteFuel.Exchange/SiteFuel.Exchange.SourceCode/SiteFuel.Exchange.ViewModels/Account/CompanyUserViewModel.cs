using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class CompanyUserViewModel
    {
        public string RegionID { get; set; }
        public string SendBirdRegionID { get; set; }

        public string RegionName { get; set; }
        public string RegionDescription { get; set; }
        public List<CompanyUserDetails> companyUserDetails { get; set; }
    }
    public class SendBirdCompanyUserViewModel
    {
        public string RegionID { get; set; }
        public string SendBirdRegionID { get; set; }

        public string RegionName { get; set; }
        public string RegionDescription { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return this.FirstName + this.LastName;
            }
        }
        public string PhoneNumber { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public string EmailAddress { get; set; }
        public string SendbirdUserName
        {
            get
            {
                return this.Role + "_" + this.EmailAddress;
            }
        }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
    }
    public class CompanyUserDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return this.FirstName + this.LastName;
            }
        }
        public string PhoneNumber { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public string EmailAddress { get; set; }
        public string SendbirdUserName
        {
            get
            {
                return this.Role + "_" + this.EmailAddress;
            }
        }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
       
    }
}
