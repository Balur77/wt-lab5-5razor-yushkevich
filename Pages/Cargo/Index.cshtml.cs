using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using wt_lab5_5razor_yushkevich.Data;
using CargoModel = wt_lab5_5razor_yushkevich.Models.Cargo;

namespace wt_lab5_5razor_yushkevich.Pages.Cargo;

public class IndexModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Category { get; set; }

    public IReadOnlyList<CargoModel> Cargoes { get; private set; } = Array.Empty<CargoModel>();
    public IEnumerable<SelectListItem> Categories { get; private set; } = Array.Empty<SelectListItem>();

    public int TotalCount => CargoStore.Cargoes.Count;
    public bool HasFilters => !string.IsNullOrWhiteSpace(Search) || !string.IsNullOrWhiteSpace(Category);

    public void OnGet()
    {
        var query = CargoStore.Cargoes.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            var term = Search.Trim();
            query = query.Where(cargo =>
                cargo.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                cargo.Location.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                cargo.Description.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(Category))
        {
            query = query.Where(cargo =>
                string.Equals(cargo.Category, Category, StringComparison.OrdinalIgnoreCase));
        }

        Cargoes = query.OrderBy(cargo => cargo.Id).ToList();

        Categories = CargoStore.Cargoes
            .Select(cargo => cargo.Category)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(category => category)
            .Select(category => new SelectListItem(category, category))
            .ToList();
    }
}
