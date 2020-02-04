using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogBlog.Models
{
    public class postViewmodel
    {
        public string id { get; set; }
        public string WriterName { get; set; }
        public string DatedPost { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string Img { get; set; }
        public string Category { get; set; }
        public string commentsNumber { get; set; }      
    }
}