using MiniWebshop.Core.Models;

namespace MiniWebshop.WebAPI.Models;

public class SetDiscountDto
{
  public string Type { get; set; } = string.Empty;
  public decimal? Percentage { get; set; }
  public decimal? Bedrag { get; set; }
  public decimal? MinTotaal { get; set; }
  public ProductCategorie? Categorie { get; set; }
}