using KatalogFilmova.Models;
using System.Xml.Linq;

namespace KatalogFilmova.Services;

public class UserServ
{
    private readonly string put;

    public UserServ(string p)
    {
        put = p;
        NapraviAkoNePostoji();
    }

    public List<Korisnik> GetAll()
    {
        var doc = XDocument.Load(put);
        return doc.Root!.Elements("user").Select(x => new Korisnik
        {
            username = (string?)x.Element("username") ?? "",
            password = (string?)x.Element("password") ?? ""
        }).ToList();
    }

    public bool Register(Korisnik k)
    {
        var lista = GetAll();
        if (lista.Any(x => x.username.ToLower() == k.username.ToLower())) return false;
        lista.Add(k);
        Sacuvaj(lista);
        return true;
    }

    public bool Login(Korisnik k)
    {
        return GetAll().Any(x => x.username == k.username && x.password == k.password);
    }

    private void Sacuvaj(List<Korisnik> lista)
    {
        var doc = new XDocument(new XElement("users",
            lista.Select(k => new XElement("user",
                new XElement("username", k.username),
                new XElement("password", k.password)
            ))));
        doc.Save(put);
    }

    private void NapraviAkoNePostoji()
    {
        var dir = Path.GetDirectoryName(put)!;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        if (!File.Exists(put)) new XDocument(new XElement("users")).Save(put);
    }
}
