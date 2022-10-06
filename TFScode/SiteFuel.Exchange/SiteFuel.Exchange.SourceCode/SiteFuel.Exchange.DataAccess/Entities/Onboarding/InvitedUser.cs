using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class InvitedUser
    {
        public InvitedUser()
        {
            MstRoles = new HashSet<MstRole>();
        }
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        [StringLength(256)]
        public string FirstName { get; set; }
        [StringLength(256)]
        public string LastName { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public int InvitedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public virtual ICollection<MstRole> MstRoles { get; set; }
        public virtual User User { get; set; }
        public virtual Company Company { get; set; }
    }
}
