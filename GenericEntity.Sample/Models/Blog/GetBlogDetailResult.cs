using System;
using System.Collections.Generic;
using GenericEntity.Sample.Models.Tag;

namespace GenericEntity.Sample.Models.Blog
{
    public class GetBlogDetailResult : GetBlogResult
    {
        public new IEnumerable<GetTagResult> Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}