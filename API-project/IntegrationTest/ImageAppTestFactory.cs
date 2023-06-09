using Infrastructure.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace IntegrationTest
{
    public class ImageAppTestFactory<TProgram>
        :WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<AppDbContext>));
                services.Remove(dbContextDescriptor);
                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbConnection));
                services.Remove(dbConnectionDescriptor);

                services
                    .AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<AppDbContext>((container, options) =>
                    {
                        options.UseInMemoryDatabase("QuizTest").UseInternalServiceProvider(container);
                    });
            });
            builder.UseEnvironment("Development");
        }
    }
}