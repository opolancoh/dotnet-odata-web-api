using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ODataExample.Web.Contracts;
using ODataExample.Web.Models;

namespace ODataExample.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _service;
    
    public BooksController(IBookService service)
    {
        _service = service;
    }

    [HttpGet]
    [EnableQuery]
    public IQueryable<Book> Get()
    {
        return _service.GetAll();
    }
}