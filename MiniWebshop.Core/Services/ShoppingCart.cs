using MiniWebshop.Core.Discounts;
using MiniWebshop.Core.Models;

namespace MiniWebshop.Core.Services;

public class ShoppingCart
{
  private readonly List<CartItem> _items = new();
  public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
  private IKortingStrategie _kortingStrategie = new GeenKorting();

  public void AddProduct(Product product, int aantal)
  {
    if (product == null) throw new ArgumentNullException(nameof(product));
    if (aantal <= 0) throw new ArgumentOutOfRangeException(nameof(aantal), "Aantal moet groter zijn dan 0");

    var bestaandItem = _items.FirstOrDefault(i => i.Product.Id == product.Id);

    if (bestaandItem != null)
    {
      bestaandItem.VerhoogAantal(aantal);
    }
    else
    {
      _items.Add(new CartItem(product, aantal));
    }
  }

  public decimal TotalPrice()
  {
    return _items.Sum(i => i.Product.Prijs * i.Aantal);
  }

  public decimal EindTotaal()
  {
    var korting = _kortingStrategie.BerekenKorting(this);
    return TotalPrice() - korting;
  }

  public void StelKortingStrategieIn(IKortingStrategie strategie)
  {
    _kortingStrategie = strategie ?? throw new ArgumentNullException(nameof(strategie));
  }

  public void Clear()
  {
    _items.Clear();
  }
}