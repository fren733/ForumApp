using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForumApplication.Models
{
    public class IsLiked
    {
        public IsLiked(int userid, int postid)
        {
            this.UserID = userid;
            this.PostID = postid;
        }
        public IsLiked() { }

        public int IsLikedID { get; set; }
        public int UserID { get; set; }
        public int PostID { get; set; }

    }
}