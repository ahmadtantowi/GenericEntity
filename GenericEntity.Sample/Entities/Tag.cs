using System.Collections.Generic;
using GenericEntity.Sample.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericEntity.Sample.Entities
{
    public class Tag : BaseEntity, ISampleEntity
    {
        public string Name { get; set; }

        public virtual ICollection<BlogTag> BlogTags { get; set; }
    }

    internal class TagEntityConfiguration : BaseEntityConfiguration<Tag>
    {
        public override void Configure(EntityTypeBuilder<Tag> builder)
        {
            base.Configure(builder);
        }
    }
}