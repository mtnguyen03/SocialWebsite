using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Gift
    {
        public int GiftId { get; set; }
        public string? GiftName { get; set; }  
        public string? Description { get; set; }  
        public int? PostId { get; set; }  
        public Post? Post { get; set; }
        public string? UserId { get; set; } 
        public User? User { get; set; } 
    }
}
