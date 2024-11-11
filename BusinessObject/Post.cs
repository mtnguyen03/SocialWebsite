using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{

    public class Post
    {
        public int PostId { get; set; }
        public string UserId { get; set; } 
        public string Content { get; set; }  
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<Comment>? Comments { get; set; } = new List<Comment>();
        public List<Like>? Likes { get; set; } = new List<Like>();
        public List<PostImage>? Images { get; set; } = new List<PostImage>();
        public List<Gift>? Gifts { get; set; }
    }
}
