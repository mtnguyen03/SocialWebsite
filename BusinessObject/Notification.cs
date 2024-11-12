using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Notification
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationID { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public string? Photo { get; set; }
        public string? MessageContent { get; set; }
        public DateTime? NotificationDate { get; set; } = DateTime.Now;
        public bool? IsRead { get; set; } = false;

        public virtual User? User { get; set; }
    }
}
