using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Tests.Integration;

internal static class CustomWebApplicationFactory
{
    public static HttpClient GetClient()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<FinanceContext>));

                services.Remove(descriptor);

                services.AddDbContext<FinanceContext>(options =>
                    options.UseInMemoryDatabase("TestDb"));
            });
        });
        
        return factory.CreateClient();
    }
}