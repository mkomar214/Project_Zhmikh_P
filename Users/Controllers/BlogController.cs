using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Users.Models;
using Users.Infrastructure;
using System.Timers;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace Users.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            BlogPostsManager manager = new BlogPostsManager();
            return View(manager.GetPostsData());
        }


        public ActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePost(PostsDb data, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                PostsDbContext context = new PostsDbContext();

                data.UserName = User.Identity.Name;

                context.PostsDb.Add(data);

                if (image != null)
                {
                    data.ImageMimeType = image.ContentType;
                    data.ImageData = new byte[image.ContentLength];

                    image.InputStream.Read(data.ImageData, 0, image.ContentLength);
                }

                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
#if false
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
#endif
                }
                return Redirect("/Blog/Index");
            }
            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeletePost(int id)
        {
            PostsDbContext context = new PostsDbContext();

            PostsDb post = new PostsDb();
            post = await context.PostsDb.FindAsync(id);

            if (post != null)
            {
                context.PostsDb.Remove(post);
                await context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return View("Error", new string[] { "Запись не найдена" });
            }
        }

        public FileContentResult GetImage(int id)
        {
            PostsDbContext context = new PostsDbContext();

            PostsDb post = new PostsDb();
            post = context.PostsDb.FirstOrDefault(x => x.Id == id);

            if (post != null)
            {
                return File(post.ImageData, post.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}