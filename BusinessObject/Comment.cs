using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }  
        public string UserId { get; set; } 
        public string Content { get; set; } 
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<CommentReply>? Replies { get; set; } = new List<CommentReply>();
    }
}
