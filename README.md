# Katalog filmova i serija

ASP.NET Core MVC aplikacija na temu katalog filmova i serija. Projekat koristi XML fajlove umesto baze podataka.

## Funkcionalnosti

- Home strana
- Lista filmova i serija
- Dodavanje, izmena i brisanje
- Pretraga po nazivu
- Filter po zanru
- Sortiranje po godini i oceni
- Paginacija
- Upload slike ili unos URL slike
- Registracija i login bez Identity framework-a
- Profil korisnika
- Export XML i JSON podataka

## XML fajlovi

- `App_Data/filmovi.xml` - podaci o filmovima i serijama
- `App_Data/users.xml` - podaci o korisnicima

## Test korisnik

- Korisnicko ime: `nikola`
- Lozinka: `1234`

## Pokretanje

1. Otvori terminal u folderu projekta:

```powershell
cd D:\projekatnikola
```

2. Pokreni aplikaciju:

```powershell
dotnet run
```

3. Otvori adresu koja se prikaze u terminalu, na primer:

```text
http://localhost:5000
```

## Napomena

Aplikacija je namerno jednostavna i pogodna za skolski projekat. Kontroleri ne rade direktno sa XML fajlom; za filmove se koristi servis klasa `FilmServ`.
