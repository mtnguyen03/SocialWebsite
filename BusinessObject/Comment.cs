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
        public int PostId { get; set; }  // Reference to the post this comment belongs to
        public string UserId { get; set; }  // Reference to the user who made the comment
        public string Content { get; set; }  // Comment content
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation property for comment replies
        public List<CommentReply>? Replies { get; set; } = new List<CommentReply>();
    }
}
