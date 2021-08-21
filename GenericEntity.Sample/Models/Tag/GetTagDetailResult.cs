using System;

namespace GenericEntity.Sample.Models.Tag
{
    public class GetTagDetailResult : GetTagResult
    {
        public int TotalBlog { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}