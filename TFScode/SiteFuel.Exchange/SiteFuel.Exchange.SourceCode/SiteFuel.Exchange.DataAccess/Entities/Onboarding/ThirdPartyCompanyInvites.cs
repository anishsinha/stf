using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class ThirdPartyCompanyInvites
    {
        [Key]
        public int Id { get; set; }
        public string UserInfo { get; set; }
        public string CompanyInfos { get; set; }
        public string FleetInfo { get; set; }
        public string ServiceOffering { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int InvitedByCompanyId { get; set; }
        public bool IsInvitedCompanyRegistered { get; set; }
        public int RegisteredCompanyId { get; set; }
    }
}
