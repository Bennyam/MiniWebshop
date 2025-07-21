using MiniWebshop.Core.Models;
using FluentAssertions;

namespace MiniWebshop.Tests.Models;

public class CartItemTests
{
  [Fact]
  public void Constructor_GooitFoutBijNullProduct()
  {
    Action act = () => new CartItem(null!, 1);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void Constructor_GooitFoutBijNegatiefAantal()
  {
    var product = new Product { Id = 1, Naam = "Test", Prijs = 5m };
    Action act = () => new CartItem(product, 0);
    act.Should().Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void VerhoogAantal_GooitFoutBijNegatief()
  {
    var product = new Product { Id = 1, Naam = "Test", Prijs = 3m };
    var item = new CartItem(product, 1);

    Action act = () => item.VerhoogAantal(0);
    act.Should().Throw<ArgumentOutOfRangeException>();
  }
}