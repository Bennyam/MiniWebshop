using Microsoft.AspNetCore.Mvc;
using MiniWebshop.WebAPI.Services;
using MiniWebshop.WebAPI.Models;
using MiniWebshop.Core.Models;
using MiniWebshop.Core.Discounts;

namespace MiniWebshop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
  private readonly ShoppinCartService _cartService;
  private readonly ProductService _productService;

  public CartController(ShoppinCartService cartService, ProductService productService)
  {
    _cartService = cartService;
    _productService = productService;
  }

  [HttpGet("items")]
  public IActionResult GetItems()
  {
    return Ok(_cartService.Cart.Items.Select(i => new
    {
      product = i.Product,
      aantal = i.Aantal
    }));
  }

  [HttpPost("add")]
  public IActionResult AddProductToCart([FromBody] AddToCartDto dto)
  {
    var product = _productService.GetById(dto.ProductId);

    if (product is null) return NotFound($"Product met ID {dto.ProductId} bestaat niet.");

    try
    {
      _cartService.Cart.AddProduct(product, dto.Aantal);
      return Ok("Product toegevoegd aan winkelmandje.");
    }
    catch (ArgumentOutOfRangeException ex)
    {
      return BadRequest(ex.Message);
    }
    catch (ArgumentNullException ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpPost("discount")]
  public IActionResult StelKortingIn([FromBody] SetDiscountDto dto)
  {
    try
    {
      IKortingStrategie strategie = dto.Type.ToLowerInvariant() switch
      {
        "geen" => new GeenKorting(),

        "percentage" => new PercentageKorting(
          percentage: dto.Percentage ?? throw new ArgumentException("Percentage is verplicht voor percentagekorting.")),

        "vast" => new VasteKorting(
          bedrag: dto.Bedrag ?? throw new ArgumentException("Bedrag is verplicht voor vaste korting."),
          minTotaal: dto.MinTotaal ?? throw new ArgumentException("MinTotaal is verplicht voor vaste korting.")),

        "percategorie" => new CategorieKorting(
          categorie: dto.Categorie ?? throw new ArgumentException("Categorie is verplicht voor categoriekorting."),
          percentage: dto.Percentage ?? throw new ArgumentException("Percentage is verplicht voor categoriekorting.")),

        _ => throw new ArgumentException("Ongeldig kortingstype.")
      };

      _cartService.StelKortingIn(strategie);
      return Ok("Korting succesvol ingesteld.");
    }
    catch (ArgumentException ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpGet("total")]
  public IActionResult GetTotaalZonderKorting()
  {
    var totaal = _cartService.Cart.TotalPrice();
    return Ok(new { TotaalZonderKorting = totaal });
  }

  [HttpGet("final")]
  public IActionResult GetEindTotaalMetKorting()
  {
    var eindTotaal = _cartService.Cart.EindTotaal();
    return Ok(new { TotaalMetKorting = eindTotaal });
  }

  [HttpPost("clear")]
  public IActionResult LeegCart()
  {
    _cartService.Cart.Clear();
    return Ok("Winkelwagentje is geleegd.");
  }
}