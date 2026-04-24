using AdminLibrary.Models;

namespace AdminLibrary.Services.Interfaces;

public interface ILoanService
{
    List<Loan> GetLoans(bool isAdmin, int userId);

    List<Book> GetAvailableBooks(int userId, bool isAdmin);

    List<User> GetUsers();

    string CreateLoan(int bookId, int userId, bool isAdmin, int? selectedUserId);

    string ReturnLoan(int loanId, int userId, bool isAdmin);
}