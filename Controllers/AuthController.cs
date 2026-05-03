using KatalogFilmova.Models;
using KatalogFilmova.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KatalogFilmova.Controllers;

public class AuthController : Controller
{
    private readonly UserServ us;

    public AuthController(IWebHostEnvironment env)
    {
        us = new UserServ(Path.Combine(env.ContentRootPath, "App_Data", "users.xml"));
    }

    public IActionResult Login()
    {
        return View(new Korisnik());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(Korisnik k)
    {
        if (!ModelState.IsValid) return View(k);

        if (!us.Login(k))
        {
            ModelState.AddModelError("", "Pogresno korisnicko ime ili lozinka.");
            return View(k);
        }

        await Prijavi(k.username);
        return RedirectToAction("Index", "Film");
    }

    public IActionResult Register()
    {
        return View(new Korisnik());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(Korisnik k)
    {
        if (!ModelState.IsValid) return View(k);

        if (!us.Register(k))
        {
            ModelState.AddModelError("username", "Korisnicko ime vec postoji.");
            return View(k);
        }

        await Prijavi(k.username);
        return RedirectToAction("Index", "Film");
    }

    [Authorize]
    public IActionResult Profile()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    private async Task Prijavi(string username)
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
        var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var user = new ClaimsPrincipal(id);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
    }
}
