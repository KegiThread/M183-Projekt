using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Role_Based_Authorization.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public List<Comment> Comments { get; set; }
    }
}