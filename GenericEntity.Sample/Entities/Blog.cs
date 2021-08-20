using System.Collections.Generic;
using GenericEntity.Sample.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericEntity.Sample.Entities
{
    public class Blog : BaseEntity, ISampleEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public virtual ICollection<BlogTag> BlogTags { get; set; }
    }

    internal class BlogEntityConfiguration : BaseEntityConfiguration<Blog>
    {
        public override void Configure(EntityTypeBuilder<Blog> builder)
        {
            base.Configure(builder);
        }
    }
}