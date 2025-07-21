using MiniWebshop.Core.Discounts;
using MiniWebshop.Core.Models;
using MiniWebshop.Core.Services;
using MiniWebshop.Tests.Helpers;
using FluentAssertions;

namespace MiniWebshop.Tests.Discounts;

public class KortingStrategieTests
{
  [Fact]
  public void GeenKorting_GeeftAltijdNull()
  {
    var cart = TestHelper.MaakCart(
      TestHelper.MaakCartItem(1, 10, 2, ProductCategorie.Boeken)
    );

    var korting = new GeenKorting().BerekenKorting(cart);
    korting.Should().Be(0);
  }

  [Fact]
  public void PercentageKorting_BerekentCorrect()
  {
    var cart = TestHelper.MaakCart(
      TestHelper.MaakCartItem(1, 20, 2)
    );

    var korting = new PercentageKorting(10).BerekenKorting(cart);
    korting.Should().Be(4);
  }

  [Fact]
  public void CategorieKorting_GeeftEnkelKortingOpGekozenCategorie()
  {
    var cart = TestHelper.MaakCart(
      TestHelper.MaakCartItem(3, 15, 2, ProductCategorie.Speelgoed),
      TestHelper.MaakCartItem(4, 25, 1, ProductCategorie.Kleding)
    );

    var korting = new CategorieKorting(ProductCategorie.Speelgoed, 50).BerekenKorting(cart);
    korting.Should().Be(15);
  }

  [Fact]
  public void VasteKorting_GeeftAlleenKortingBijMinimumTotaal()
  {
    var cart = TestHelper.MaakCart(
      TestHelper.MaakCartItem(5, 20, 2)
    );

    var strategie = new VasteKorting(10, minTotaal: 50);
    var korting = strategie.BerekenKorting(cart);
    korting.Should().Be(0);

    cart.AddProduct(TestHelper.MaakProduct(6, 10), 2);
    korting = strategie.BerekenKorting(cart);
    korting.Should().Be(10);
  }
}