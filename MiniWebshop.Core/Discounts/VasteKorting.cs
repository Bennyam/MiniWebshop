using MiniWebshop.Core.Services;

namespace MiniWebshop.Core.Discounts;

public class VasteKorting : IKortingStrategie
{
  private readonly decimal _bedrag;
  private readonly decimal _minTotaal;

  public VasteKorting(decimal bedrag, decimal minTotaal = 0)
  {
    if (bedrag <= 0)
      throw new ArgumentOutOfRangeException(nameof(bedrag), "Korting moet groter zijn dan 0");

    _bedrag = bedrag;
    _minTotaal = minTotaal;
  }

  public decimal BerekenKorting(ShoppingCart cart)
  {
    return cart.TotalPrice() >= _minTotaal ? _bedrag : 0;
  }
}