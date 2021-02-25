using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForumApplication.Models
{
    public class PostCreateModel
    {
        public int OverPostID { get; set; }
        public int PostID { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Date { get; set; }
        public int CommentsNum { get; set; }
        public string UserID { get; set; }
        public string Username { get; set; }
        public string ProfileImage { get; set; }
        public int Likes { get; set; }
        public bool IsLiked { get; set; }
    }
}