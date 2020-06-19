using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogBlog.Models
{
    public class postViewmodel
    {
        public string id { get; set; }
        public string WriterName { get; set; }
        public string DatedPost { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [AllowHtml]
        public string Text { get; set; }
        public string Img { get; set; }
        public string Category { get; set; }
        public string quote { get; set; }
        [AllowHtml]
        public string text2 { get; set; }
        public string commentsNumber { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Post> post { get; set; }
        public Nullable<long> Viewed { get; set; }
    }
}