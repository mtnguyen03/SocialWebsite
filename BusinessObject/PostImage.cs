using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PostImage
    {
        public int PostImageId { get; set; } 
        public int PostId { get; set; }  
        public string ImageUrl { get; set; }  
        public string? Description { get; set; } 
        public int? DisplayOrder { get; set; } 
        public Post? Post { get; set; }
    }
}
