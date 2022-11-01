using ODataExample.Web.Models;

namespace ODataExample.Web.Data;

public class DbSeeder
{
    public static Guid BookId1 => new Guid("734c6506-a293-4d78-b04d-08dab93deb38");
    public static Guid BookId2 => new Guid("3e8ec853-8ac5-4d28-b04e-08dab93deb38");
    public static Guid BookId3 => new Guid("bc651af7-1de2-4a79-b04f-08dab93deb38");
    public static Guid BookId4 => new Guid("0acf646f-5a7f-4f5c-b050-08dab93deb38");
    
    public static List<Book> Books => new List<Book>
    {
        new Book()
        {
            Id = BookId1,
            Title = "Book 01",
            PublishedOn = new DateTime(2021, 01, 24)
        },
        new Book()
        {
            Id = BookId2,
            Title = "Book 02",
            PublishedOn = new DateTime(2021, 04, 26)
        },
        new Book()
        {
            Id = BookId3,
            Title = "Book 03",
            PublishedOn = new DateTime(2022, 07, 13)
        },
        new Book()
        {
            Id = BookId4,
            Title = "Book 04",
            PublishedOn = new DateTime(2022, 10, 20)
        }
    };

    public static List<Review> Reviews => new List<Review>
    {
        new Review()
        {
            Id = new Guid("F3F8CB55-D333-4359-1741-08DABB9DE4CD"),
            BookId = BookId1,
            Comment = "Comment 01",
            Rating = 1
        },
        new Review()
        {
            Id = new Guid("92E5FD7A-EFEE-41F5-1742-08DABB9DE4CD"),
            BookId = BookId1,
            Comment = "Comment 02",
            Rating = 2
        },
        new Review()
        {
            Id = new Guid("E6404220-A6FD-49D7-1743-08DABB9DE4CD"),
            BookId = BookId2,
            Comment = "Comment 03",
            Rating = 3
        },
        new Review()
        {
            Id = new Guid("3B73C3A1-BDEE-4AF7-1744-08DABB9DE4CD"),
            BookId = BookId2,
            Comment = "Comment 04",
            Rating = 4
        },
        new Review()
        {
            Id = new Guid("D59735EC-2221-4613-1745-08DABB9DE4CD"),
            BookId = BookId3,
            Comment = "Comment 05",
            Rating = 5
        },
        new Review()
        {
            Id = new Guid("BFF16FDC-A7C9-49AD-1746-08DABB9DE4CD"),
            BookId = BookId3,
            Comment = "Comment 06",
            Rating = 5
        },
        new Review()
        {
            Id = new Guid("92E0F255-EF02-42A5-1747-08DABB9DE4CD"),
            BookId = BookId4,
            Comment = "Comment 07",
            Rating = 4
        },
        new Review()
        {
            Id = new Guid("9DD18902-3398-47EF-1748-08DABB9DE4CD"),
            BookId = BookId4,
            Comment = "Comment 08",
            Rating = 3
        }
    };

    public static void Seed(WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.Books.AddRange(Books);
        db.Reviews.AddRange(Reviews);

        db.SaveChanges();
    }

}