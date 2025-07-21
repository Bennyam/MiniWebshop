namespace MiniWebshop.Core.Models;

public class CartItem
{
  public Product Product { get; }
  public int Aantal { get; private set; }

  public CartItem(Product product, int aantal)
  {
    if (aantal <= 0) throw new ArgumentOutOfRangeException(nameof(aantal), "Aantal moet groter zijn dan 0");

    Product = product ?? throw new ArgumentNullException(nameof(product));
    Aantal = aantal;
  }

  public void VerhoogAantal(int extra)
  {
    if (extra <= 0) throw new ArgumentOutOfRangeException(nameof(extra), "Extra aantal moet groter zijn dan 0");
    Aantal += extra;
  }
}