using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public class PushNotificationLog
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int NotificationCode { get; set; }

        public int AppTypeId { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string JsonMessage { get; set; }
        public bool IsActive { get; set; }
        public bool IsRead { get; set; }
    }
}
