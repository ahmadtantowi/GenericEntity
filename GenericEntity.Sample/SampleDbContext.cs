using GenericEntity.Abstractions;
using GenericEntity.Extensions;
using GenericEntity.Sample.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GenericEntity.Sample
{
    public class SampleDbContext : AppDbContext<SampleDbContext>, ISampleDbContext
    {
        public SampleDbContext() : base() { }
        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyEntityRegistration();
        }

        DbSet<UEntity> IAppDbContext<ISampleEntity>.Entity<UEntity>()
        {
            return Set<UEntity>();
        }
    }
}