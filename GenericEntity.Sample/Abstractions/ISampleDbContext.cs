using GenericEntity.Abstractions;

namespace GenericEntity.Sample.Abstractions
{
    public interface ISampleDbContext : IAppDbContext<ISampleEntity> { }
}