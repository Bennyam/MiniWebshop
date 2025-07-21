using MiniWebshop.Core.Models;
using MiniWebshop.Core.Services;

namespace MiniWebshop.Core.Discounts;

public class PercentageKorting : IKortingStrategie
{
  private readonly decimal _percentage;

  public PercentageKorting(decimal percentage)
  {
    if (percentage < 0 || percentage > 100)
      throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage moet tussen 0 en 100 zijn");

    _percentage = percentage;
  }

  public decimal BerekenKorting(ShoppingCart cart)
  {
    return cart.TotalPrice() * (_percentage / 100);
  }
}