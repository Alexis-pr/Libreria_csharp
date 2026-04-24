using AdminLibrary.Data;
using AdminLibrary.Services.Interfaces;
using AdminLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminLibrary.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _Context;
    
    public BookService(AppDbContext context)
    {
        _Context = context;
    }
    
    public List<Book> GetAll()
    {
        return _Context.Books.ToList();
    }

    public void Add(Book book)
    {
        _Context.Books.Add(book);
        _Context.SaveChanges();
    }

    public Book GetById(int id)
    {
        return _Context.Books.Find(id);
    }

    public void Update(Book book)
    {
        _Context.Books.Update(book);
        _Context.SaveChanges();
    }

    public void Delete(int id)
    {
        var book = _Context.Books.Find(id);
        if (book != null)
        {
            _Context.Books.Remove(book);
            _Context.SaveChanges();
        }
    }
}
    
    
    
