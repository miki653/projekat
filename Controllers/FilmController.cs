using KatalogFilmova.Models;
using KatalogFilmova.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KatalogFilmova.Controllers;

public class FilmController : Controller
{
    private readonly FilmServ fs;
    private readonly IWebHostEnvironment env;

    public FilmController(IWebHostEnvironment e)
    {
        env = e;
        fs = new FilmServ(Path.Combine(env.ContentRootPath, "App_Data", "filmovi.xml"));
    }

    public IActionResult Index(string? q, string? zanr, string? sort, int p = 1)
    {
        var lista = fs.GetAll();

        if (!string.IsNullOrWhiteSpace(q))
            lista = lista.Where(x => x.naz.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!string.IsNullOrWhiteSpace(zanr))
            lista = lista.Where(x => x.zanr == zanr).ToList();

        lista = sort switch
        {
            "god_asc" => lista.OrderBy(x => x.god).ToList(),
            "god_desc" => lista.OrderByDescending(x => x.god).ToList(),
            "oc_asc" => lista.OrderBy(x => x.oc).ToList(),
            "oc_desc" => lista.OrderByDescending(x => x.oc).ToList(),
            _ => lista.OrderBy(x => x.naz).ToList()
        };

        int poStrani = 6;
        int ukupno = (int)Math.Ceiling(lista.Count / (double)poStrani);
        if (p < 1) p = 1;
        if (ukupno > 0 && p > ukupno) p = ukupno;

        ViewBag.q = q;
        ViewBag.zanr = zanr;
        ViewBag.sort = sort;
        ViewBag.p = p;
        ViewBag.ukupno = ukupno;
        ViewBag.zanrovi = fs.GetAll().Select(x => x.zanr).Distinct().OrderBy(x => x).ToList();

        return View(lista.Skip((p - 1) * poStrani).Take(poStrani).ToList());
    }

    [Authorize]
    public IActionResult Create()
    {
        return View(new Film { god = DateTime.Now.Year, oc = 8, tip = "film" });
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Film f, IFormFile? slika)
    {
        if (!ModelState.IsValid) return View(f);
        f.img = SnimiSliku(slika, f.img);
        fs.Add(f);
        return RedirectToAction("Index");
    }

    [Authorize]
    public IActionResult Edit(int id)
    {
        var f = fs.GetAll().FirstOrDefault(x => x.id == id);
        if (f == null) return NotFound();
        return View(f);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Film f, IFormFile? slika)
    {
        if (!ModelState.IsValid) return View(f);
        f.img = SnimiSliku(slika, f.img);
        fs.Update(f);
        return RedirectToAction("Index");
    }

    [Authorize]
    public IActionResult Delete(int id)
    {
        var f = fs.GetAll().FirstOrDefault(x => x.id == id);
        if (f == null) return NotFound();
        return View(f);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteOk(int id)
    {
        fs.Delete(id);
        return RedirectToAction("Index");
    }

    public IActionResult ExportXml()
    {
        return Content(fs.GetXml().ToString(), "application/xml");
    }

    public IActionResult ExportJson()
    {
        return Json(fs.GetAll());
    }

    private string SnimiSliku(IFormFile? slika, string? url)
    {
        if (slika == null || slika.Length == 0) return url ?? "";

        var dir = Path.Combine(env.WebRootPath, "uploads");
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        var ext = Path.GetExtension(slika.FileName);
        var ime = Guid.NewGuid().ToString("N") + ext;
        var put = Path.Combine(dir, ime);
        using var tok = new FileStream(put, FileMode.Create);
        slika.CopyTo(tok);
        return "/uploads/" + ime;
    }
}
