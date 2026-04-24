using AdminLibrary.Data;
using AdminLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Controllers;

[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var users = _context.Users.OrderBy(u => u.Username).ToList();
        return View(users);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username) ||
            string.IsNullOrWhiteSpace(user.Email) ||
            string.IsNullOrWhiteSpace(user.Password))
        {
            ModelState.AddModelError("", "Todos los campos son obligatorios");
            return View(user);
        }

        if (_context.Users.Any(u => u.Email == user.Email))
        {
            ModelState.AddModelError("", "Ya existe un usuario con ese email");
            return View(user);
        }

        if (string.IsNullOrWhiteSpace(user.Role)) user.Role = "User";
        _context.Users.Add(user);
        _context.SaveChanges();

        TempData["Success"] = $"Usuario \"{user.Username}\" creado.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost]
    public IActionResult Edit(User user)
    {
        var existing = _context.Users.Find(user.id);
        if (existing == null) return NotFound();

        // No permitir quitarle Admin al último admin
        if (existing.Role == "Admin" && user.Role == "User")
        {
            var adminCount = _context.Users.Count(u => u.Role == "Admin");
            if (adminCount <= 1)
            {
                TempData["Error"] = "No puedes cambiar el rol: debe haber al menos un administrador.";
                return RedirectToAction("Index");
            }
        }

        existing.Username = user.Username;
        existing.Email = user.Email;
        existing.Role = user.Role;
        if (!string.IsNullOrWhiteSpace(user.Password))
            existing.Password = user.Password;

        _context.SaveChanges();
        TempData["Success"] = $"Usuario \"{user.Username}\" actualizado.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var currentUserId = int.Parse(User.FindFirst("UserId")!.Value);
        if (id == currentUserId)
        {
            TempData["Error"] = "No puedes eliminar tu propio usuario.";
            return RedirectToAction("Index");
        }

        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        // Evitar eliminar el último admin
        if (user.Role == "Admin" && _context.Users.Count(u => u.Role == "Admin") <= 1)
        {
            TempData["Error"] = "No puedes eliminar el único administrador.";
            return RedirectToAction("Index");
        }

        _context.Users.Remove(user);
        _context.SaveChanges();

        TempData["Success"] = $"Usuario \"{user.Username}\" eliminado.";
        return RedirectToAction("Index");
    }
}
