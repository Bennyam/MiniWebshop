using MiniWebshop.Core.Models;

namespace MiniWebshop.WebAPI.Services;

public class ProductService
{
  private readonly List<Product> _producten = new()
  {
    new Product { Id = 1, Naam = "Boek", Prijs = 12.50m, Voorraad = 50, Categorie = ProductCategorie.Boeken },
    new Product { Id = 2, Naam = "T-shirt", Prijs = 25m, Voorraad = 30, Categorie = ProductCategorie.Kleding },
    new Product { Id = 3, Naam = "Lego Set", Prijs = 45m, Voorraad = 20, Categorie = ProductCategorie.Speelgoed },
    new Product { Id = 4, Naam = "Smartphone", Prijs = 399m, Voorraad = 10, Categorie = ProductCategorie.Electronica }
  };

  public IEnumerable<Product> GetAll() => _producten;

  public Product? GetById(int id) => _producten.FirstOrDefault(p => p.Id == id);
}
