using System.Threading;
using System.Threading.Tasks;
using GenericEntity.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace GenericEntity
{
    public abstract class AppDbContext<TContext> : DbContext, IAppDbContext where TContext : DbContext
    {
        public DatabaseFacade DbFacade => Database;

        public AppDbContext() : base() {} 
        public AppDbContext(DbContextOptions<TContext> options) : base(options) { }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            return Database.BeginTransactionAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected abstract override void OnModelCreating(ModelBuilder modelBuilder);
    }
}