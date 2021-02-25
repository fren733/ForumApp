using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ForumApplication.Models;

namespace ForumApplication.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Register() => View();

        [HttpPost]
        public ActionResult Register(User user)
        {
            Models.Helpers repository = new Models.Helpers();
            if (repository.RegisterUser(user))
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Login() => View();

        [HttpPost]
        public ActionResult Login(User user)
        {
            Models.Helpers repository = new Models.Helpers();
            if (repository.SingInUser(user))
            {
                FormsAuthentication.RedirectFromLoginPage(user.Username, false);
            }

            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            Session.Abandon();
            Request.Cookies.Clear();
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize]
        public ActionResult UserProfile(string Username)
        {
            if (Username == null) Username = User.Identity.Name;

            UserHelperModel userModel = new UserHelperModel();
            Models.Helpers repository = new Models.Helpers();

            string imgsrc;
            var user = repository.GetUser(Username);

            if (user.Avatar == null)
                imgsrc = "";
            else
            {
                var base64 = Convert.ToBase64String(user.Avatar);
                imgsrc = string.Format($"data:image/gif;base64,{base64}");
            }
            userModel.Avatar = imgsrc;

            if (user.BackgroundImage == null)
                imgsrc = "";
            else
            {
                var base64 = Convert.ToBase64String(user.BackgroundImage);
                imgsrc = string.Format($"data:image/gif;base64,{base64}");
            }
            userModel.BackgroundImage = imgsrc;
            userModel.Username = user.Username;


            return View(userModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult UserProfile(HttpPostedFileBase ProfileImageFile, HttpPostedFileBase BackgroundImageFile)
        {
            byte[] profile = null;
            byte[] background = null;

            if (ProfileImageFile != null)
            {
                using (Stream input = ProfileImageFile.InputStream)
                {
                    MemoryStream memory = input as MemoryStream;
                    if (memory == null)
                    {
                        memory = new MemoryStream();
                        input.CopyTo(memory);
                    }
                    profile = memory.ToArray();
                }
            }

            if (BackgroundImageFile != null)
            {
                using (Stream input = BackgroundImageFile.InputStream)
                {
                    MemoryStream memory = input as MemoryStream;
                    if (memory == null)
                    {
                        memory = new MemoryStream();
                        input.CopyTo(memory);
                    }
                    background = memory.ToArray();
                }
            }

            using (ForumContext context = new ForumContext())
            {
                User user = context.Users.SingleOrDefault(x => x.Username.Equals(User.Identity.Name));

                if (ProfileImageFile != null)
                    user.Avatar = profile;

                if (BackgroundImageFile != null)
                    user.BackgroundImage = background;

                context.SaveChanges();
            }

            return RedirectToAction("UserProfile");
        }

        public JsonResult GetUserPosts(string username, int pageNumber, int pageSize)
        {
            Models.Helpers repository = new Models.Helpers();
            List<PostCreateModel> postList = new List<PostCreateModel>();

            var items = repository.GetPosts(pageNumber, pageSize, username);
            string imgsrc;

            foreach (Post item in items)
            {
                if (item.ImageFile == null)
                    imgsrc = "";
                else
                {
                    var base64 = Convert.ToBase64String(item.ImageFile);
                    imgsrc = string.Format($"data:image/gif;base64,{base64}");
                }

                PostCreateModel postModel = new PostCreateModel
                {
                    PostID = item.PostID,
                    Content = item.Content,
                    Date = item.PublicationDate.ToString(),
                    Image = imgsrc
                };

                postList.Add(postModel);
            }

            return Json(postList, JsonRequestBehavior.AllowGet);
        }
    }
}