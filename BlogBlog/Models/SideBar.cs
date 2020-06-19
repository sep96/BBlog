using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogBlog.Models
{
    public class SideBar
    {
        public List<Post> post { get; set; }
        public List<NumberCategory> sidebar { get; set; }
        public List<Category> category { get; set; }
    }
}