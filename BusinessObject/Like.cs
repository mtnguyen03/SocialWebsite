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
        public int PostId { get; set; }  // Reference to the post this like belongs to
        public string UserId { get; set; }  // Reference to the user who liked the post
        public DateTime? CreatedAt { get; set; }
    }
}
