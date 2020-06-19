using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BlogBlog.Models;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace BlogBlog.Controllers
{
    public class PostsController : Controller
    {
        private BlogDBEntities db = new BlogDBEntities();

        // GET: Posts
        public ActionResult Index(int page = 1)
        {
            int recordsPerPage = 4;
            
            IEnumerable<postViewmodel> posts =  (from item in db.Posts
                join item2 in db.Categories on (item.CategoryId) equals item2.categoryId
                select new postViewmodel
                {
                    id = item.Id.ToString(),
                    WriterName =  item.WriterName,
                    DatedPost = item.DatedPost,
                    Title = item.Title,
                    Description =  item.Description,
                    Text =  item.Text,
                    Img = item.Img,
                    Category = item2.Name,
                    Viewed = item.Viewed,
                    commentsNumber = (from item3 in db.Comments where item3.PostId == item.Id && item3.Status==true select item3).Count().ToString()
                }).ToList().ToPagedList(page, recordsPerPage); 
            return View(posts);
        }

        // GET: Posts/Details/5
        public ActionResult Details(long id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces()
                                where nic.OperationalStatus == OperationalStatus.Up
                                select nic.GetPhysicalAddress().ToString()
                            ).FirstOrDefault();
            var remoteIpAddress = Request.UserHostAddress;
            postViewmodel post = (from item in db.Posts
                                    join item2 in db.Categories on (item.CategoryId) equals item2.categoryId
                                    where item.Id == id 
                                    select new postViewmodel
                                    {
                                        id = item.Id.ToString(),
                                        WriterName = item.WriterName,
                                        DatedPost = item.DatedPost,
                                        Title = item.Title,
                                        Description = item.Description,
                                        Text = item.Text,
                                        Img = item.Img,
                                        Category = item2.Name,
                                        Viewed = item.Viewed,
                                        text2 = item.TextsecondPart,
                                        quote = item.quotes,
                                        commentsNumber = (from item3 in db.Comments where item3.PostId == item.Id && item3.Status == true select item3).Count().ToString(),
                                        Comments = (from item3 in db.Comments where item3.PostId == item.Id && item3.Status == true select item3).ToList(),
                                        post = (from item3 in db.Posts where ((item3.Id.Equals((id - 1))) || (item3.Id.Equals((id + 1))))
                                                    select item3).ToList()
                                    }).FirstOrDefault();
            long temp = Convert.ToInt64(post.id);
            Post result = db.Posts.First(e => e.Id == temp);
            result.Viewed ++;
            db.Entry(result).State = EntityState.Modified;
            db.SaveChanges();
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "categoryId", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,WriterName,DatedPost,Title,Description,Text,Img,CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Viewed = 0;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "categoryId", "Name", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "categoryId", "Name", post.CategoryId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,WriterName,DatedPost,Title,Description,Text,Img,CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "categoryId", "Name", post.CategoryId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }
        [ChildActionOnly]
        public ActionResult SideBar()
        {
            var tempsidebar =  (from item in db.Posts
                join item2 in db.Categories on item.CategoryId equals item2.categoryId
                group item2 by item2.Name
                into g
                select new NumberCategory
                {
                    Name = g.Key,
                    count = g.Count()
                }).ToList();
            SideBar result = new SideBar
            {
                post =  (from item in db.Posts orderby item.Viewed descending select item).Take(3).ToList(),
                sidebar = tempsidebar,
                category =  db.Categories.ToList()
            };
            return PartialView(result);
        }
        [HttpPost]
        public string comment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                DateTime d = DateTime.Now;
                PersianCalendar pc = new PersianCalendar();
                comment.Date = string.Format("{0}/{1}/{2}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d));
                db.Comments.Add(comment);
                db.SaveChanges();
                return "ok";
            }
            return null;
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
