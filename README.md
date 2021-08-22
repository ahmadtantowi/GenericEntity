# Generic Entity
A library to manage EF Core entity easier without adding `DbSet<T>` of every single entity into database context

[![NuGet](https://img.shields.io/nuget/v/GenericEntity.svg?label=NuGet)](https://www.nuget.org/packages/GenericEntity)

## Features
- No need to write each entity field on DbContext
- Support multiple database
- Cleaner query invoke

## Setup
First, create interface that represent entity of database, then implement `IEntity`<br>
```csharp
public interface ISampleEntity : IEntity { }
```
Second, create interface that represent db context that implement `IAppDbContext<TEntity>`, where T is interface of entity that created before
```csharp
public interface ISampleDbContext : IAppDbContext<ISampleEntity> { }
``` 
Third, create entity class that impelement interface entity above
```csharp
public class Blog : ISampleEntity
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}
```
Forth, create db context class that inherit from `AppDbContext<TContext>` and implement interface db context that created before, where `TContext` is db context itself
```csharp
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
```
__Note:__ You need to override `OnModelCreating` and `Entity` method like above to make this library work

`ApplyEntityRegistration()` will make sure each class that implement interface entity (first step) is migrated into database, and `Set<T>` is to return field entity on db context.

Lastly (optional), register interface db context and db context into service container
```csharp
services.AddScoped<ISampleDbContext, SampleDbContext>();
```

## Usage
Instead of inject db context instance (when using DI), you can inject interface db context (second step) into constructor
```csharp
public class BlogController : Controller
{
    private readonly ISampleDbContext _dbContext;

    public BlogController(ISampleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    ...
}
```
And then invoke method `Entity<TEntity>` when query to some entity
```csharp
public class BlogController : Controller
{
    ...

    [HttpGet("{id}")]
    public async Task<IActionResult> ReadDetail(string id, CancellationToken ct)
    {
        var blog = await _dbContext.Entity<Blog>()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(ct);

        return new OkObjectResult(blog);
    }
}
```
