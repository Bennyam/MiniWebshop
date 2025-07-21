using MiniWebshop.Core.Services;

namespace MiniWebshop.Core.Discounts;

public class GeenKorting : IKortingStrategie
{
  public decimal BerekenKorting(ShoppingCart cart)
  {
    return 0;
  }
}