using ODataExample.Web.Models;

namespace ODataExample.Web.Contracts;

public interface IBookService
{
    public IQueryable<Book> GetAll();
}