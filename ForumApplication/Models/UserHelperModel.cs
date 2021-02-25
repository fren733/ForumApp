using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForumApplication.Models
{
    public class UserHelperModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string BackgroundImage { get; set; }
        public IEnumerable<Post> Posts;
    }
}