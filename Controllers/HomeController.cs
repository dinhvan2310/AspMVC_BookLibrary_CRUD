using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_BookLibrary.Models;

namespace MVC_BookLibrary.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Services _service;

    public HomeController(ILogger<HomeController> logger, Services services)
    {
        _logger = logger;
        _service = services;
    }

    public IActionResult Index(string searchString, int page = 1, string orderName = "Id", bool isDes = false)
    {
        switch(orderName) {
            case "Id": {
                ViewData["Id"] = !(isDes);
                break;
            }
            case "Title": {
                ViewData["TieuDe"] = !(isDes);
                break;
            }
            case "Author": {
                ViewData["Author"] = !(isDes);
                break;
            }
            case "Publisher": {
                ViewData["Publisher"] = !(isDes);
                break;
            }
            case "PublishYear": {
                ViewData["PublishYear"] = !(isDes);
                break;
            }
        }

        if (searchString != null)
        {
            ViewData["searchString"] = searchString;
            var model = _service.page(_service.getListBook(searchString).ToList(), page, orderName, isDes);
            ViewData["page"] = model.Item3;
            ViewData["pages"] = model.Item2;
            return View(model.Item1);
        }
        else
        {
            ViewData["searchString"] = searchString;
            var model = _service.page(_service.getListBook().ToList(), page, orderName, isDes);
            ViewData["page"] = model.Item3;
            ViewData["pages"] = model.Item2;
            return View(model.Item1);
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Detail(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        else
        {
            if (_service.getBook(id.Value) == null)
            {
                return NotFound();
            }
            return View(_service.getBook(id.Value));
        }
    }

    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        else
        {
            return View(_service.getBook(id.Value));
        }
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        if (!_service.deleteBook(id))
        { }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        else
        {
            if (_service.getBook(id.Value) == null)
            {
                return NotFound();
            }
            return View(_service.getBook(id.Value));
        }
    }
    [HttpPost]
    public IActionResult Edit(BookModel book, IFormFile file)
    {
        if (ModelState.IsValid)
        {
            book.DataFile = _service.getBook(book.Id).DataFile;
            _service.upLoadBook(book, file);
            _service.updateBook(book);
            return RedirectToAction("Index");
        }
        return View(book);
    }

    public IActionResult Dowload(int id)
    {
        var book = _service.getBook(id);
        if (book == null) return NotFound();
        string fileName = book.DataFile;
        string path = Path.Combine("Data", fileName);
        if (!System.IO.File.Exists(path)) return NotFound();

        var file = System.IO.File.ReadAllBytes(path);

        var type = Path.GetExtension(book.DataFile) switch
        {
            "pdf" => "application/pdf",
            "docx" => "application/vnd.ms-word",
            "doc" => "application/vnd.ms-word",
            "txt" => "text/plain",
            _ => "application/pdf"
        };

        return File(file, type, book.DataFile);
    }

    public IActionResult Create()
    {
        BookModel book = new BookModel();
        return View(book);
    }

    [HttpPost]
    public IActionResult Create(BookModel book, IFormFile file)
    {
        if(ModelState.IsValid)
        {
            _service.upLoadBook(book, file);
            _service.addBook(book);
            return RedirectToAction("Index");
        }
        return View(book);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}


