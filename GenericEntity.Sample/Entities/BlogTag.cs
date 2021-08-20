using GenericEntity.Sample.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericEntity.Sample.Entities
{
    public class BlogTag : BaseEntity, ISampleEntity
    {
        public string BlogId { get; set; }
        public string TagId { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual Tag Tag { get; set; }
    }

    internal class BlogTagConfiguration : BaseEntityConfiguration<BlogTag>
    {
        public override void Configure(EntityTypeBuilder<BlogTag> builder)
        {
            builder.HasOne(x => x.Blog)
                .WithMany(x => x.BlogTags)
                .HasForeignKey(fk => fk.BlogId);

            builder.HasOne(x => x.Tag)
                .WithMany(x => x.BlogTags)
                .HasForeignKey(fk => fk.TagId);

            base.Configure(builder);
        }
    }
}