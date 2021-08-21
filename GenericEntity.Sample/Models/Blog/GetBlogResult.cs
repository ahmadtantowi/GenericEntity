using System.Collections.Generic;

namespace GenericEntity.Sample.Models.Blog
{
    public class GetBlogResult : UniqueId
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}