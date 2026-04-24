using AdminLibrary.Data;
using Microsoft.EntityFrameworkCore;
using AdminLibrary.Models;
using AdminLibrary.Services.Interfaces;

namespace AdminLibrary.Services;

public class LoanService : ILoanService
{
    private readonly AppDbContext _context;

    public LoanService(AppDbContext context)
    {
        _context = context;
    }

    public List<Loan> GetLoans(bool isAdmin, int userId)
    {
        if (isAdmin)
        {
            return _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .OrderByDescending(l => l.LoanDate)
                .ToList();
        }

        return _context.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.LoanDate)
            .ToList();
    }

    public List<Book> GetAvailableBooks(int userId, bool isAdmin)
    {
        if (isAdmin)
            return _context.Books.Where(b => b.Stock > 0).ToList();

        var prestadosIds = _context.Loans
            .Where(l => l.UserId == userId)
            .Select(l => l.BookId)
            .ToList();

        return _context.Books
            .Where(b => b.Stock > 0 && !prestadosIds.Contains(b.Id))
            .ToList();
    }

    public List<User> GetUsers()
    {
        return _context.Users
            .OrderBy(u => u.Username)
            .ToList();
    }

    public string CreateLoan(int bookId, int userId, bool isAdmin, int? selectedUserId)
    {
        if (isAdmin && selectedUserId.HasValue)
            userId = selectedUserId.Value;

        var exists = _context.Loans
            .FirstOrDefault(l => l.UserId == userId && l.BookId == bookId);

        if (exists != null)
            return "Este usuario ya tiene este libro";

        var book = _context.Books.FirstOrDefault(b => b.Id == bookId);

        if (book == null)
            return "Libro no encontrado";

        if (book.Stock <= 0)
            return "No hay stock disponible";

        _context.Loans.Add(new Loan
        {
            UserId = userId,
            BookId = bookId,
            LoanDate = DateTime.Now
        });

        book.Stock -= 1;

        _context.SaveChanges();

        return "OK";
    }

    public string ReturnLoan(int loanId, int userId, bool isAdmin)
    {
        Loan? loan;

        if (isAdmin)
        {
            loan = _context.Loans
                .Include(l => l.Book)
                .FirstOrDefault(l => l.Id == loanId);
        }
        else
        {
            loan = _context.Loans
                .Include(l => l.Book)
                .FirstOrDefault(l => l.Id == loanId && l.UserId == userId);
        }

        if (loan == null)
            return "No encontrado";

        loan.Book.Stock += 1;

        _context.Loans.Remove(loan);

        _context.SaveChanges();

        return "OK";
    }
}