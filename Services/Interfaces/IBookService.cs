using AdminLibrary.Models;
using AdminLibrary.DTOs;

namespace AdminLibrary.Services.Interfaces;

public interface IBookService
{
    public List<Book> GetAll();
    public void Add(Book book);
    public Book GetById(int id);
    void Update(Book book);
    void Delete(int id);
    
   
    
    
    
}