    using AdminLibrary.DTOs.Loan;
using AdminLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminLibrary.Controllers;

[Authorize]
public class LoanController : Controller
{
    private readonly ILoanService _loanService;

    public LoanController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    public IActionResult Index()
    {
        var isAdmin = User.IsInRole("Admin");
        var userId = int.Parse(User.FindFirst("UserId")!.Value);

        var loans = _loanService.GetLoans(isAdmin, userId);

        ViewBag.IsAdmin = isAdmin;

        return View(loans);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var isAdmin = User.IsInRole("Admin");
        var userId = int.Parse(User.FindFirst("UserId")!.Value);

        ViewBag.Books = _loanService.GetAvailableBooks(userId, isAdmin);

        if (isAdmin)
            ViewBag.Users = _loanService.GetUsers();

        ViewBag.IsAdmin = isAdmin;

        return View();
    }

    [HttpPost]
    public IActionResult Create(CreateLoanDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var isAdmin = User.IsInRole("Admin");
        var userId = int.Parse(User.FindFirst("UserId")!.Value);

        var result = _loanService.CreateLoan(
            dto.BookId,
            userId,
            isAdmin,
            dto.UserId
        );

        if (result != "OK")
        {
            TempData["Error"] = result;
            return RedirectToAction("Create");
        }

        TempData["Success"] = "Préstamo creado correctamente";

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Return(int id)
    {
        var isAdmin = User.IsInRole("Admin");
        var userId = int.Parse(User.FindFirst("UserId")!.Value);

        var result = _loanService.ReturnLoan(id, userId, isAdmin);

        if (result != "OK")
        {
            TempData["Error"] = result;
        }

        return RedirectToAction("Index");
    }
}