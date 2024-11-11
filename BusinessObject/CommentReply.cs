using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CommentReply
    {
        public int Id { get; set; }
        public int ReplyId { get; set; }
        public int CommentId { get; set; }  // Reference to the comment this reply belongs to
        public string UserId { get; set; }  // Reference to the user who made the reply
        public string Content { get; set; }  // Reply content
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
