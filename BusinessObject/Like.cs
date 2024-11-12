using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Like
    {
        public int LikeId { get; set; }
        public int PostId { get; set; }  
        public string UserId { get; set; }  
        public DateTime? CreatedAt { get; set; }
    }
}
