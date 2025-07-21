using MiniWebshop.Core.Services;
using MiniWebshop.Core.Discounts;

namespace MiniWebshop.WebAPI.Services;

public class ShoppinCartService
{
  private readonly ShoppingCart _cart = new();
  public ShoppingCart Cart => _cart;

  public void StelKortingIn(IKortingStrategie strategie)
  {
    _cart.StelKortingStrategieIn(strategie);
  }
}