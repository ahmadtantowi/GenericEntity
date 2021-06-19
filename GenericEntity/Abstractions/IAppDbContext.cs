using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace GenericEntity.Abstractions
{
    public interface IAppDbContext
    {
        DatabaseFacade DbFacade { get; }
        
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
    
    public interface IAppDbContext<TEntity> : IAppDbContext 
        where TEntity : class, IEntity
    {
        DbSet<UEntity> Entity<UEntity>() where UEntity : class, TEntity;
    }
}