namespace MiniWebshop.Core.Models;

public class CartItem
{
  public Product Product { get; }
  public int Aantal { get; private set; }

  public CartItem(Product product, int aantal)
  {
    Product = product;
    Aantal = aantal;
  }

  public void VerhoogAantal(int extra)
  {
    Aantal += extra;
  }
}