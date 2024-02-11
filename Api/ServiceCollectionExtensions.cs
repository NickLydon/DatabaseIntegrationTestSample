using Database;
using Microsoft.EntityFrameworkCore;

namespace DatabaseIntegrationTestSample;

internal static class ServiceCollectionExtensions
{
    public static void AddSampleDbContext<TImplementation>(this IServiceCollection services)
        where TImplementation : SampleDbContext
    {
        services.AddDbContext<SampleDbContext, TImplementation>((_, optionsBuilder) =>
            optionsBuilder.UseSqlServer("name=ConnectionStrings:SampleDb"));
    }
}