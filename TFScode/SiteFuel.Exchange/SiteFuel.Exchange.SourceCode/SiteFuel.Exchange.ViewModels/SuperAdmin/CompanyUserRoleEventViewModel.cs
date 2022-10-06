using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class CompanyUserRoleEventViewModel
    {
        public CompanyUserRoleEventViewModel()
        {
            BuyerUsers = new List<int>();
            SupplierUsers = new List<int>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<int> BuyerUsers { get; set; }

        public List<int> SupplierUsers { get; set; }

        public bool IsForBuyerUsers { get; set; }

        public bool IsForSupplierUsers { get; set; }

        public bool IsEmail { get; set; }

        public bool IsSms { get; set; }

        public bool IsEmailEnabled { get; set; }

        public bool IsSmsEnabled { get; set; }
    }

    public class NotificationSettingsViewModel
    {
        public NotificationSettingsViewModel()
        {
            EventGroupIds = new List<int>();
            EventGroupDetails = new List<NotificationGroupViewModel>();
        }
        public List<int> EventGroupIds { get; set; }

        public List<NotificationGroupViewModel> EventGroupDetails { get; set; }
    }

    public class NotificationGroupViewModel
    {
        public NotificationGroupViewModel()
        {
            EventDetails = new List<CompanyUserRoleEventViewModel>();
        }


        public string EventGroupName { get; set; }

        public List<CompanyUserRoleEventViewModel> EventDetails { get; set; }
    }
}
