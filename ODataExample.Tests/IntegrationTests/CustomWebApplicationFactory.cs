using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ODataExample.Web.Data;

namespace ODataExample.Tests.IntegrationTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove current DbContext
            var serviceDescriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (serviceDescriptor != null) services.Remove(serviceDescriptor);

            // Add DbContext for testing
            services.AddDbContext<ApplicationDbContext>(options => options
                .UseSqlServer("Server=localhost,1433;Database=BooksOdataDBTest;User Id=sa;Password=My@Passw0rd"));

            //
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            logger.LogInformation("The test database 'BooksOdataDBTest' should have been created and populated");
        });
    }
}