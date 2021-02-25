using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForumApplication.Models
{
    public class Post
    {
        [Key]
        public int PostID { get; set; }
        public string Content { get; set; }
        public byte[] ImageFile { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool IsComment { get; set; }
        public int Likes { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}