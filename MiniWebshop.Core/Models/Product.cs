namespace MiniWebshop.Core.Models;

public class Product
{
  public int Id { get; set; }
  public required string Naam { get; set; }
  public decimal Prijs { get; set; }
  public int Voorraad { get; set; }
  public ProductCategorie Categorie { get; set; }
}
