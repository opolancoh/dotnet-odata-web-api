using ODataExample.Web.Contracts;
using ODataExample.Web.Data;
using ODataExample.Web.Models;

namespace ODataExample.Web.Services;

public class BookService : IBookService
{
    private readonly ApplicationDbContext _context;

    public BookService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Book> GetAll()
    {
        return _context.Books
            .AsQueryable();
    }
}