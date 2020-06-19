﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BlogBlog.Models;

namespace BlogBlog.Controllers
{
    public class HomeController : Controller
    {
        private BlogDBEntities db = new BlogDBEntities();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
   
    }
}
