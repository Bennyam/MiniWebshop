using Microsoft.AspNetCore.Mvc;
using MiniWebshop.Core.Models;
using MiniWebshop.WebAPI.Services;

namespace MiniWebshop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
  private readonly ProductService _productService;

  public ProductController(ProductService productService)
  {
    _productService = productService;
  }

  [HttpGet]
  public ActionResult<IEnumerable<Product>> GetAll()
  {
    return Ok(_productService.GetAll());
  }

  [HttpGet("{id}")]
  public ActionResult<Product> GetById(int id)
  {
    var product = _productService.GetById(id);
    if (product is null)
      return NotFound($"Product met ID {id} niet gevonden.");

    return Ok(product);
  }
}
