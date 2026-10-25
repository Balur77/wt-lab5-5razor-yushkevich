using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab5_5razor_yushkevich.Data;
using CargoModel = wt_lab5_5razor_yushkevich.Models.Cargo;

namespace wt_lab5_5razor_yushkevich.Pages.Cargo;

public class CreateModel : PageModel
{
    [BindProperty]
    public CargoModel Cargo { get; set; } = new();

    public IEnumerable<string> Categories => CargoStore.Cargoes
        .Select(cargo => cargo.Category)
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .OrderBy(category => category);

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Cargo.Id = CargoStore.GetNextId();
        Cargo.Name = Cargo.Name.Trim();
        Cargo.Category = Cargo.Category.Trim();
        Cargo.Location = Cargo.Location.Trim();
        Cargo.Description = Cargo.Description.Trim();

        CargoStore.Cargoes.Add(Cargo);

        TempData["SuccessMessage"] =
            $"Груз «{Cargo.Name}» добавлен в каталог (ID: {Cargo.Id}).";

        return RedirectToPage("./Index");
    }
}
