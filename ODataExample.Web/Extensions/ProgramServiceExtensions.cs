using Microsoft.EntityFrameworkCore;
using ODataExample.Web.Contracts;
using ODataExample.Web.Data;
using ODataExample.Web.Services;

namespace ODataExample.Web.Extensions;

public static class ProgramServiceExtensions
{
    public static void ConfigureDbContext(this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddDbContext<ApplicationDbContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("SqlServerConnection")));

    public static void ConfigureEmployeeService(this IServiceCollection services) =>
        services.AddScoped<IBookService, BookService>();
}