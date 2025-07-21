using MiniWebshop.Core.Models;
using MiniWebshop.Core.Services;

namespace MiniWebshop.Core.Discounts;

public class CategorieKorting : IKortingStrategie
{
  private readonly ProductCategorie _categorie;
  private readonly decimal _percentage;

  public CategorieKorting(ProductCategorie categorie, decimal percentage)
  {
    if (percentage < 0 || percentage > 100)
      throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage moet tussen 0 en 100 zijn.");

    _categorie = categorie;
    _percentage = percentage;
  }

  public decimal BerekenKorting(ShoppingCart cart)
  {
    var totaalInCategorie = cart.Items
        .Where(item => item.Product.Categorie == _categorie)
        .Sum(item => item.Product.Prijs * item.Aantal);

    return totaalInCategorie * (_percentage / 100);
  }

}