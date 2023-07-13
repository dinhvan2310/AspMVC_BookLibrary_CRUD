using MVC_BookLibrary.Models;
using System.Linq.Dynamic.Core;

public class Services
{
    private readonly Context _context;
    private readonly ILogger<Services> _logger;
    private List<BookModel> listBook {set; get;}

    public Services(Context context, ILogger<Services> logger)
    {
        _context = context;
        _logger = logger;
        listBook = _context.BookModels.ToList();
    }

    public List<BookModel> getListBook() => listBook;
    public List<BookModel> getListBook(string searchString)
    {
        searchString = searchString.ToLower();
        return _context.BookModels.Where(b => b.Title.ToLower().Contains(searchString) ||
                                    b.Author.ToLower().Contains(searchString) ||
                                    b.Publisher.ToLower().Contains(searchString) ||
                                    b.PublishYear.ToString().ToLower().Contains(searchString)
                            ).ToList();
    }
    public BookModel getBook(int id) => _context.BookModels.Where(b => b.Id == id).FirstOrDefault();

    public bool deleteBook(int id)
    {
        BookModel temp = _context.BookModels.Find(id);
        if(temp == null)
        {
            return false;
        }
        if(temp.DataFile != null)
        {
            string path = "Data\\" + temp.DataFile;
            if(File.Exists(path))
            {
                File.Delete(path);
            }
        }
        _context.BookModels.Remove(temp);
        _context.SaveChanges();
        return true;
    }

    public void updateBook(BookModel book)
    {
        var temp = _context.BookModels.Find(book.Id);
        _context.BookModels.Remove(temp);
        _context.BookModels.Add(book);
        _context.SaveChanges();
    }

    public void upLoadBook(BookModel book, IFormFile file)
    {
        if(!Directory.Exists("Data"))
        {
            Directory.CreateDirectory("Data");
        }
        string path = "Data\\" + file.FileName;
        using var fileStream = new FileStream(path, FileMode.Create);
        file.CopyTo(fileStream);
        if(!String.IsNullOrEmpty(book.DataFile))
        {
            string pathFileRemove = "Data\\" + book.DataFile;
            if(File.Exists(pathFileRemove))
            {
                File.Delete(pathFileRemove);
            }
        }
        book.DataFile = file.FileName;
    }

    public void addBook(BookModel book)
    {
        _context.BookModels.Add(book);
        _context.SaveChanges();
    }

    public (List<BookModel>, int, int) page(List<BookModel> books ,int page, string orderName = "Title", bool isDes = false)
    {
        int pages = (int)Math.Ceiling((decimal)books.Count() / 10);
        List<BookModel> rsBook = books.AsQueryable()
                                .OrderBy($"{orderName} {(isDes?"Descending":"")}")
                                .Skip((page - 1)*10).Take(10).ToList();
        return (rsBook, pages, page);
    }

}