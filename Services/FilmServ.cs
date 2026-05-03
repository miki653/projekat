using KatalogFilmova.Models;
using System.Xml.Linq;

namespace KatalogFilmova.Services;

public class FilmServ
{
    private readonly string put;

    public FilmServ(string p)
    {
        put = p;
        NapraviAkoNePostoji();
    }

    public List<Film> GetAll()
    {
        var doc = XDocument.Load(put);
        return doc.Root!.Elements("film").Select(x => new Film
        {
            id = (int)x.Element("id")!,
            naz = (string?)x.Element("naz") ?? "",
            zanr = (string?)x.Element("zanr") ?? "",
            god = (int)x.Element("god")!,
            oc = (double)x.Element("oc")!,
            opis = (string?)x.Element("opis") ?? "",
            img = (string?)x.Element("img") ?? "",
            tip = (string?)x.Element("tip") ?? "film"
        }).ToList();
    }

    public void Add(Film f)
    {
        var lista = GetAll();
        f.id = lista.Count == 0 ? 1 : lista.Max(x => x.id) + 1;
        lista.Add(f);
        Sacuvaj(lista);
    }

    public void Update(Film f)
    {
        var lista = GetAll();
        var stari = lista.FirstOrDefault(x => x.id == f.id);
        if (stari == null) return;

        stari.naz = f.naz;
        stari.zanr = f.zanr;
        stari.god = f.god;
        stari.oc = f.oc;
        stari.opis = f.opis;
        stari.img = f.img;
        stari.tip = f.tip;
        Sacuvaj(lista);
    }

    public void Delete(int id)
    {
        var lista = GetAll();
        var f = lista.FirstOrDefault(x => x.id == id);
        if (f != null) lista.Remove(f);
        Sacuvaj(lista);
    }

    public XDocument GetXml()
    {
        return XDocument.Load(put);
    }

    private void Sacuvaj(List<Film> lista)
    {
        var doc = new XDocument(new XElement("filmovi",
            lista.Select(f => new XElement("film",
                new XElement("id", f.id),
                new XElement("naz", f.naz),
                new XElement("zanr", f.zanr),
                new XElement("god", f.god),
                new XElement("oc", f.oc),
                new XElement("opis", f.opis),
                new XElement("img", f.img ?? ""),
                new XElement("tip", f.tip)
            ))));
        doc.Save(put);
    }

    private void NapraviAkoNePostoji()
    {
        var dir = Path.GetDirectoryName(put)!;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        if (!File.Exists(put)) new XDocument(new XElement("filmovi")).Save(put);
    }
}
