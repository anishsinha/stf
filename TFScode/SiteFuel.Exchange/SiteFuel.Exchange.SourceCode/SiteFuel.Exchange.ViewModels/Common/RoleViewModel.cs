using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class RoleViewModel : StatusViewModel
    {
        public RoleViewModel()
        {
        }

        public RoleViewModel(Status status)
            : base(status)
        {
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
