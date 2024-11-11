using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PostImage
    {
        public int PostImageId { get; set; }  // Unique ID for the image
        public int PostId { get; set; }  // Reference to the post this image belongs to
        public string ImageUrl { get; set; }  // URL or path to the image
        public string? Description { get; set; }  // Optional description of the image
        public int? DisplayOrder { get; set; }  // Optional, to control the order in which images appear

        // Navigation property back to the post
        public Post? Post { get; set; }
    }
}
