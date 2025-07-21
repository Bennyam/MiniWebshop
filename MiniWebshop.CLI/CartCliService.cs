using MiniWebshop.Core.Discounts;
using MiniWebshop.Core.Models;
using MiniWebshop.Core.Services;
using Spectre.Console;

namespace MiniWebshop.CLI;

public class CartCliService
{
  private readonly ShoppingCart _cart = new();

  private readonly List<Product> _producten = new()
  {
      new Product { Id = 1, Naam = "Boek", Prijs = 12.50m, Voorraad = 50, Categorie = ProductCategorie.Boeken },
      new Product { Id = 2, Naam = "T-shirt", Prijs = 25m, Voorraad = 30, Categorie = ProductCategorie.Kleding },
      new Product { Id = 3, Naam = "Lego Set", Prijs = 45m, Voorraad = 20, Categorie = ProductCategorie.Speelgoed },
      new Product { Id = 4, Naam = "Smartphone", Prijs = 399m, Voorraad = 10, Categorie = ProductCategorie.Electronica }
  };

  public void Start()
  {
      while (true)
      {
          AnsiConsole.Clear();
          var keuze = AnsiConsole.Prompt(
              new SelectionPrompt<string>()
                  .Title("[yellow]Wat wil je doen?[/]")
                  .AddChoices(new[]
                  {
                      "Product toevoegen",
                      "Winkelmandje bekijken",
                      "Korting instellen",
                      "Mandje leegmaken",
                      "Afsluiten"
                  }));

          switch (keuze)
          {
              case "Product toevoegen": VoegProductToe(); break;
              case "Winkelmandje bekijken": ToonCart(); break;
              case "Korting instellen": StelKortingIn(); break;
              case "Mandje leegmaken": _cart.Clear(); break;
              case "Afsluiten": return;
          }
      }
  }

  private void VoegProductToe()
  {
    var product = AnsiConsole.Prompt(
        new SelectionPrompt<Product>()
            .Title("[green]Kies een product:[/]")
            .UseConverter(p => $"{p.Naam} (€{p.Prijs})")
            .AddChoices(_producten));

    var aantal = AnsiConsole.Ask<int>("[green]Aantal:[/]");

    try
    {
      _cart.AddProduct(product, aantal);
      AnsiConsole.MarkupLine("[blue]Product toegevoegd aan mandje.[/]");
    }
    catch (Exception ex)
    {
      AnsiConsole.MarkupLine($"[red]Fout: {ex.Message}[/]");
    }

    Console.WriteLine();
    AnsiConsole.MarkupLine("[grey]Druk op een toets om verder te gaan...[/]");
    Console.ReadKey(true);
  }

  private void ToonCart()
  {
    AnsiConsole.Clear();
    if (!_cart.Items.Any())
    {
      AnsiConsole.MarkupLine("[italic]Winkelmandje is leeg.[/]");
      Console.ReadKey(true);
      return;
    }

    var table = new Table().RoundedBorder();
    table.AddColumn("Product");
    table.AddColumn("Aantal");
    table.AddColumn("Subtotaal");

    foreach (var item in _cart.Items)
    {
      table.AddRow(item.Product.Naam, item.Aantal.ToString(), $"€{item.Product.Prijs * item.Aantal:N2}");
    }

    table.AddRow("[bold]Totaal[/]", "", $"[bold]€{_cart.TotalPrice():N2}[/]");
    table.AddRow("[bold]Eindtotaal met korting[/]", "", $"[green]€{_cart.EindTotaal():N2}[/]");

    AnsiConsole.Write(table);
    Console.WriteLine();
    AnsiConsole.MarkupLine("[grey]Druk op een toets om verder te gaan...[/]");
    Console.ReadKey(true);   
  }

  private void StelKortingIn()
  {
    var keuze = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[blue]Kies een kortingstype:[/]")
            .AddChoices("Geen", "Percentage", "Vast", "Per Categorie"));

    IKortingStrategie strategie = keuze switch
    {
      "Geen" => new GeenKorting(),
      "Percentage" => new PercentageKorting(AnsiConsole.Ask<decimal>("Percentage (0-100):")),
      "Vast" => new VasteKorting(
          bedrag: AnsiConsole.Ask<decimal>("Bedrag:"),
          minTotaal: AnsiConsole.Ask<decimal>("Minimum totaal voor korting:")),
      "Per Categorie" => new CategorieKorting(
          categorie: AnsiConsole.Prompt(new SelectionPrompt<ProductCategorie>()
              .Title("Kies categorie:")
              .AddChoices(Enum.GetValues<ProductCategorie>())),
          percentage: AnsiConsole.Ask<decimal>("Percentage (0-100):")),
      _ => new GeenKorting()
    };

    _cart.StelKortingStrategieIn(strategie);
    AnsiConsole.MarkupLine("[blue]Korting ingesteld.[/]");
    Console.WriteLine();
    AnsiConsole.MarkupLine("[grey]Druk op een toets om verder te gaan...[/]");
    Console.ReadKey(true);
  }
}
