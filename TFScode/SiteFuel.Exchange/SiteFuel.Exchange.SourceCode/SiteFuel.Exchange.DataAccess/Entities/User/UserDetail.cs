using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class UserDetail
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [StringLength(256)]
        public string Title { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
