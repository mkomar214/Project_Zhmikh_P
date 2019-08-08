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
using System.Collections;

namespace Users.Infrastructure
{
    public class BlogPostsManager
    {
        public Stack<PostsDb> GetPostsData()
        {
            PostsDbContext context = new PostsDbContext();
            Stack<PostsDb> stack = new Stack<PostsDb>();

            if (context.PostsDb.Count() != 0)
            {
                foreach (PostsDb db in context.PostsDb)
                {
                    stack.Push(db);
                }
            }

            return stack;
        }
    }
}