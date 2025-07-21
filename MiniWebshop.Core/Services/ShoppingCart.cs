using MiniWebshop.Core.Models;

namespace MiniWebshop.Core.Services;

public class ShoppinCart
{
  private readonly List<CartItem> _items = new();
  public IReadOnlyList<CartItem> Items => _items;

  public void AddProduct(Product product, int aantal)
  {
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

  public void Clear()
  {
    _items.Clear();
  }
}