using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab5_5razor_yushkevich.Data;
using CargoModel = wt_lab5_5razor_yushkevich.Models.Cargo;

namespace wt_lab5_5razor_yushkevich.Pages;

public class IndexModel : PageModel
{
    public IReadOnlyList<CargoModel> Cargoes { get; private set; } = Array.Empty<CargoModel>();
    public IReadOnlyList<CargoModel> RecentCargoes { get; private set; } = Array.Empty<CargoModel>();

    public int PositionsCount => Cargoes.Count;
    public int AvailableCount => Cargoes.Count(cargo => cargo.IsAvailable);
    public int TotalQuantity => Cargoes.Sum(cargo => cargo.Quantity);
    public decimal TotalWeight => Cargoes.Sum(cargo => cargo.TotalWeight);

    public void OnGet()
    {
        Cargoes = CargoStore.Cargoes;
        RecentCargoes = CargoStore.Cargoes
            .OrderByDescending(cargo => cargo.Id)
            .Take(3)
            .ToList();
    }
}
