using AdminLibrary.Models;
using AdminLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminLibrary.Controllers;

[Authorize]
public class BookController : Controller
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    public IActionResult Index()
    {
        var books = _bookService.GetAll();
        return View(books);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Book book)
    {
        if (!ModelState.IsValid) return View(book);
        _bookService.Add(book);
        TempData["Success"] = "Libro creado correctamente.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var book = _bookService.GetById(id);
        if (book == null) return NotFound();
        return View(book);
    }

    [HttpPost]
    public IActionResult Edit(Book book)
    {
        if (!ModelState.IsValid) return View(book);
        _bookService.Update(book);
        TempData["Success"] = "Libro actualizado correctamente.";
        return RedirectToAction("Index");
    }

    // ✅ FIX: Delete es POST, el <a> en la vista se reemplaza por un form
    [HttpPost]
    public IActionResult Delete(int id)
    {
        _bookService.Delete(id);
        TempData["Success"] = "Libro eliminado.";
        return RedirectToAction("Index");
    }
}
