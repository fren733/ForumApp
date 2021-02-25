using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForumApplication.Models;
using ForumApplication.Helpers;

namespace ForumApplication.Controllers
{
    public class PostController : Controller
    {
        [HttpGet]
        public ActionResult PostNo(int ID)
        {
            using (ForumContext context = new ForumContext())
            {
                Post overPost = context.Posts.FirstOrDefault(x => x.PostID.Equals(ID));
                User user = context.Users.Find(overPost.UserId);

                PostCreateModel postModel = new PostCreateModel
                {
                    PostID = overPost.PostID,
                    Content = overPost.Content,
                    Date = overPost.PublicationDate.ToString(),
                    Username = user.Username
                };

                if (user.Avatar != null)
                {
                    string base64 = Convert.ToBase64String(user.Avatar);
                    string imgsrc = string.Format($"data:image/gif;base64,{base64}");
                    postModel.ProfileImage = imgsrc;
                }

                if (overPost.ImageFile != null)
                {
                    string base64 = Convert.ToBase64String(overPost.ImageFile);
                    string imgsrc = string.Format($"data:image/gif;base64,{base64}");
                    postModel.Image = imgsrc;
                }


                return View(postModel);
            }
        }

        [HttpGet]
        [Authorize]
        public PartialViewResult CreatePost(int PostID)
        {
            using (ForumContext context = new ForumContext())
            {
                Post overPost = context.Posts.FirstOrDefault(x => x.PostID.Equals(PostID));
                User user = context.Users.Find(overPost.UserId);

                PostCreateModel postModel = new PostCreateModel
                {
                    PostID = overPost.PostID,
                    Content = overPost.Content,
                    Date = overPost.PublicationDate.ToString(),
                    Username = user.Username
                };

                if (user.Avatar != null)
                {
                    string base64 = Convert.ToBase64String(user.Avatar);
                    string imgsrc = string.Format($"data:image/gif;base64,{base64}");
                    postModel.ProfileImage = imgsrc;
                }

                if (overPost.ImageFile != null)
                {
                    string base64 = Convert.ToBase64String(overPost.ImageFile);
                    string imgsrc = string.Format($"data:image/gif;base64,{base64}");
                    postModel.Image = imgsrc;
                }

                return PartialView("CreatePost", postModel);
            }
        }

        [HttpGet]
        public PartialViewResult CardPartial(int PostID)
        {
            using (ForumContext context = new ForumContext())
            {
                var overPost = context.Posts.FirstOrDefault(x => x.PostID.Equals(PostID));
                var postAuthor = context.Users.FirstOrDefault(x => x.UserID == overPost.UserId);

                Models.Helpers repository = new Models.Helpers();
                User loggedUser = repository.GetUser(User.Identity.Name);
                bool isLiked = false;

                if (loggedUser != null)
                    repository.IsLiked(overPost.PostID, loggedUser.UserID);

                PostCreateModel postModel = new PostCreateModel
                {
                    PostID = overPost.PostID,
                    Content = overPost.Content,
                    Date = ViewHelpers.DateCounter(overPost.PublicationDate),
                    CommentsNum = overPost.Posts.Count,
                    Username = postAuthor.Username,
                    IsLiked = isLiked,
                    Likes = overPost.Likes
                };

                if (postAuthor.Avatar != null)
                {
                    var base64 = Convert.ToBase64String(postAuthor.Avatar);
                    var imgsrc = string.Format($"data:image/gif;base64,{base64}");
                    postModel.ProfileImage = imgsrc;
                }
                else
                {
                    postModel.ProfileImage = null;
                }

                if (overPost.ImageFile != null)
                {
                    var base64 = Convert.ToBase64String(overPost.ImageFile);
                    var imgsrc = string.Format($"data:image/gif;base64,{base64}");
                    postModel.Image = imgsrc;
                }
                else
                {
                    postModel.Image = null;
                }

                return PartialView("CardPartial", postModel);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreatePost(HttpPostedFileBase ImageFile = null, int PostID = -1)
        {
            byte[] data;
            Post overPost;
            string username = HttpContext.User.Identity.Name;

            if (ImageFile != null)
            {
                using (Stream inputStream = ImageFile.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                }
            }
            else
            {
                data = null;
            }

            using (ForumContext context = new ForumContext())
            {
                User user = context.Users.FirstOrDefault(x => x.Username.Equals(username));

                Post newPost = new Post()
                {
                    Content = Request["Content"],
                    User = user,
                    PublicationDate = DateTime.Now,
                    ImageFile = data
                };

                if (PostID != -1)
                {
                    overPost = context.Posts.FirstOrDefault(x => x.PostID.Equals(PostID));
                    newPost.IsComment = true;
                    overPost.Posts.Add(newPost);
                }

                context.Posts.Add(newPost);
                context.SaveChanges();
            }

            return RedirectToAction("PostWall");
        }

        [HttpGet]
        public ActionResult PostWall() => View();

        [HttpPost]
        [Authorize]
        public JsonResult Like(int PostID)
        {
            Models.Helpers repository = new Models.Helpers();
            int userid = repository.GetUser(User.Identity.Name.ToString()).UserID;

            int result = repository.Like(PostID, userid);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPosts(int pageNumber, int pageSize)
        {
            Models.Helpers repository = new Models.Helpers();
            List<PostCreateModel> postList = new List<PostCreateModel>();

            var items = repository.GetPosts(pageNumber, pageSize);

            foreach (var item in items)
            {
                PostCreateModel postModel = new PostCreateModel
                {
                    PostID = item.PostID
                };

                postList.Add(postModel);
            }
            return Json(postList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComments(int OverPostID, int pageNumber, int pageSize)
        {
            Models.Helpers repository = new Models.Helpers();
            List<PostCreateModel> posts = new List<PostCreateModel>();

            foreach (var item in repository.GetPosts(OverPostID, pageNumber, pageSize))
            {
                PostCreateModel postModel = new PostCreateModel
                {
                    PostID = item.PostID,
                    Content = item.Content,
                    Date = item.PublicationDate.ToString()
                };

                if (item.ImageFile != null)
                {
                    var base64 = Convert.ToBase64String(item.ImageFile);
                    var imgsrc = string.Format($"data:image/gif;base64,{base64}");
                    postModel.Image = imgsrc;
                }
                else
                {
                    postModel.Image = null;
                }
                posts.Add(postModel);
            }

            return Json(posts, JsonRequestBehavior.AllowGet);
        }


    }
}
