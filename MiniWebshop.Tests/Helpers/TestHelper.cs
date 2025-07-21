using MiniWebshop.Core.Models;
using MiniWebshop.Core.Services;

namespace MiniWebshop.Tests.Helpers;

public static class TestHelper
{
  public static Product MaakProduct(int id, decimal prijs, ProductCategorie categorie = ProductCategorie.Onbekend)
  {
    return new Product
    {
      Id = id,
      Naam = $"Product{id}",
      Prijs = prijs,
      Voorraad = 100,
      Categorie = categorie,
    };
  }

  public static CartItem MaakCartItem(int id, decimal prijs, int aantal, ProductCategorie categorie = ProductCategorie.Onbekend)
  {
    return new CartItem(MaakProduct(id, prijs, categorie), aantal);
  }

  public static ShoppingCart MaakCart(params CartItem[] items)
  {
    var cart = new ShoppingCart();
    foreach (var item in items)
    {
      cart.AddProduct(item.Product, item.Aantal);
    }
    return cart;
  }
} 