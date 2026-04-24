using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using AdminLibrary.DTOs;
using AdminLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[AllowAnonymous]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Book");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = _authService.ValidateUser(dto.Email, dto.Password);

        if (user == null)
        {
            ViewBag.Error = "Credenciales inválidas";
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),   // ✅ Rol en claims
            new Claim("UserId", user.id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties
        );

        return RedirectToAction("Index", "Book");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Book");
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        if (dto.Password != dto.ConfirmPassword)
        {
            ModelState.AddModelError("", "Las contraseñas no coinciden");
            return View(dto);
        }

        try
        {
            _authService.Add(dto);
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", e.InnerException?.Message ?? e.Message);
            return View(dto);
        }

        TempData["Success"] = "Cuenta creada. Inicia sesión.";
        return RedirectToAction("Login");
    }
}
