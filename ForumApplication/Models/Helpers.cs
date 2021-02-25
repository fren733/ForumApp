using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ForumApplication.Models;

namespace ForumApplication.Models
{
    public class Helpers
    {

        public bool SingInUser(User user)
        {
            int count;

            using (ForumContext context = new ForumContext())
            {
                List<int> users = (
                              from dbuser in context.Users
                              where dbuser.Username == user.Username &
                                    dbuser.Password == user.Password
                              select dbuser.UserID
                             ).
                             ToList();

                count = users.Count;
            }
            if (count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RegisterUser(User user)
        {
            int count;

            using (ForumContext context = new ForumContext())
            {
                var users = (
                              from dbuser in context.Users
                              where user.Username == dbuser.Username &
                                    user.Password == dbuser.Password &
                                    user.Email == dbuser.Email
                              select dbuser.UserID
                             );
                count = users.ToList().Count;

                if (count > 0)
                {
                    return false;
                }
                else
                {
                    context.Users.Add(user);
                    context.SaveChanges();

                    return true;
                }
            }
        }

        public bool CreatePost(Post post)
        {
            try
            {
                using (ForumContext context = new ForumContext())
                {
                    context.Posts.Add(post);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public User GetUser(string username)
        {
            using (ForumContext context = new ForumContext())
            {
                List<User> user;
                user = (
                            from u in context.Users
                            where u.Username == username
                            select u
                            )
                            .ToList();
                return user.FirstOrDefault();
            }
            
        }

        public User GetUser(int UserID)
        {
            using (ForumContext context = new ForumContext())
            {
                User user = context.Users.Where(x => x.UserID == UserID).FirstOrDefault();
                return user;
            }
        }

        public Post GetPost(int PostID)
        {
            using (ForumContext context = new ForumContext())
            {
                Post post = context.Posts.Where(x => x.PostID == PostID).FirstOrDefault();
                return post;
            }
        }

        public List<Post> GetPosts(int pageNumber, int pageSize)
        {
            using (ForumContext context = new ForumContext())
            {
                int startRow = ((pageNumber - 1) * pageSize);
                List<Post> posts;

                posts = context.Posts
                            .Where(x => x.IsComment != true)
                            .OrderByDescending(x => x.PublicationDate)
                            .Skip(startRow)
                            .Take(pageSize)
                            .ToList();

                return posts;
            }
        }

        public List<Post> GetPosts(int pageNumber, int pageSize, string username)
        {
            using (ForumContext context = new ForumContext())
            {
                int startRow = ((pageNumber - 1) * pageSize);
                List<Post> posts;

                posts = context.Posts
                            .Where(x => x.User.Username == username)
                            .OrderByDescending(x => x.PublicationDate)
                            .Skip(startRow)
                            .Take(pageSize)
                            .ToList();

                return posts;
            }
        }

        public List<Post> GetPosts(int PostID, int pageNumber, int pageSize)
        {
            List<Post> posts;
            int startRow = ((pageNumber - 1) * pageSize);

            using(ForumContext context = new ForumContext())
            {
                posts = context.Posts
                               .Where(x => x.PostID == PostID)
                               .FirstOrDefault()
                               .Posts
                               .OrderByDescending(x => x.PublicationDate)
                               .Skip(startRow)
                               .Take(pageSize)
                               .ToList();
            }

            return posts;
        }

        public int Like(int PostID, int UserID)
        {
            using (ForumContext context = new ForumContext())
            {
                Post post = context.Posts.Where(x => x.PostID == PostID).FirstOrDefault();
                User user = context.Users.Where(x => x.UserID == UserID).FirstOrDefault();
                IsLiked like = context.Likes.Where(x => x.PostID == PostID).Where(y => y.UserID == user.UserID).FirstOrDefault();

                if (like == null)
                {
                    post.Likes += 1;
                    IsLiked newLike = new IsLiked(UserID, PostID);
                    context.Likes.Add(newLike);
                    context.SaveChanges();

                    return post.Likes;
                }
                else
                {
                    post.Likes -= 1;
                    context.Likes.Remove(like);
                    context.SaveChanges();

                    return post.Likes;
                }
            }
        }

        public bool IsLiked(int PostID, int UserID)
        {
            using (ForumContext context = new ForumContext())
            {
                List<IsLiked> likes = context.Likes.Where(x => x.PostID == PostID).ToList();
                foreach (var like in likes)
                {
                    if (like.UserID == UserID)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}