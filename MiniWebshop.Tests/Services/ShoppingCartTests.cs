using MiniWebshop.Core.Models;
using MiniWebshop.Core.Services;
using FluentAssertions;

namespace MiniWebshop.Tests.Services;

public class ShoppingCartTests
{
    [Fact]
    public void AddProduct_VoegtNieuwItemToe()
    {
        var product = new Product { Id = 1, Naam = "Boek", Prijs = 10.0m, Voorraad = 5 };
        var cart = new ShoppingCart();

        cart.AddProduct(product, 2);

        cart.Items.Should().HaveCount(1);
        cart.Items[0].Aantal.Should().Be(2);
        cart.Items[0].Product.Naam.Should().Be("Boek");
    }

    [Fact]
    public void AadProduct_VerhoogdAantalAlsItemBestaat()
    {
        var product = new Product { Id = 1, Naam = "Laptop", Prijs = 999.99m, Voorraad = 10 };
        var cart = new ShoppingCart();

        cart.AddProduct(product, 1);
        cart.AddProduct(product, 2);

        cart.Items.Should().HaveCount(1);
        cart.Items[0].Aantal.Should().Be(3);
    }

    [Fact]
    public void TotalPrice_BerekentCorrecteSom()
    {
        var p1 = new Product { Id = 1, Naam = "Boek", Prijs = 12m };
        var p2 = new Product { Id = 2, Naam = "Pen", Prijs = 2m };
        var cart = new ShoppingCart();

        cart.AddProduct(p1, 2);
        cart.AddProduct(p2, 3);

        cart.TotalPrice().Should().Be(30m);
    }

    [Fact]
    public void Clear_MaaktCartLeeg()
    {
        var product = new Product { Id = 1, Naam = "Tv", Prijs = 120m };
        var cart = new ShoppingCart();

        cart.AddProduct(product, 1);
        cart.Clear();

        cart.Items.Should().BeEmpty();
    }

    [Fact]
    public void AddProduct_GooitFoutBijNullProduct()
    {
        var cart = new ShoppingCart();
        Action act = () => cart.AddProduct(null!, 1);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddProduct_GooitFoutBijNegatiefAantal()
    {
        var product = new Product { Id = 1, Naam = "Test", Prijs = 10m };
        var cart = new ShoppingCart();

        Action act = () => cart.AddProduct(product, 0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
