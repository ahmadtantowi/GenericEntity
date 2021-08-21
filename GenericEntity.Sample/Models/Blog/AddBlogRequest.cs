using System.Collections.Generic;

namespace GenericEntity.Sample.Models.Blog
{
    public class AddBlogRequest
    {
        public IEnumerable<string> TagIds { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}