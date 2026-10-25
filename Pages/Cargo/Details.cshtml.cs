using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab5_5razor_yushkevich.Data;
using CargoModel = wt_lab5_5razor_yushkevich.Models.Cargo;

namespace wt_lab5_5razor_yushkevich.Pages.Cargo;

public class DetailsModel : PageModel
{
    public CargoModel? Cargo { get; set; }

    public IActionResult OnGet(int id)
    {
        Cargo = CargoStore.Cargoes.FirstOrDefault(cargo => cargo.Id == id);

        if (Cargo is null)
        {
            return NotFound();
        }

        return Page();
    }
}
