using MiniWebshop.Core.Services;

namespace MiniWebshop.Core.Discounts;

public interface IKortingStrategie
{
  decimal BerekenKorting(ShoppingCart cart);
}