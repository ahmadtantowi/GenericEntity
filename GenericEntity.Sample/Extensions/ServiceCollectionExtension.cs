using GenericEntity.Sample.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericEntity.Sample.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<SampleDbContext>(options => options.UseSqlite("Data Source=sample.db"));
            services.AddScoped<ISampleDbContext, SampleDbContext>();

            return services;
        }
    }
}