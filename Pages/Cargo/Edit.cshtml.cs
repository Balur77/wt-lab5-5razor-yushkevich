using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab5_5razor_yushkevich.Data;
using CargoModel = wt_lab5_5razor_yushkevich.Models.Cargo;

namespace wt_lab5_5razor_yushkevich.Pages.Cargo;

public class EditModel : PageModel
{
    [BindProperty]
    public CargoModel Cargo { get; set; } = new();

    public IEnumerable<string> Categories => CargoStore.Cargoes
        .Select(cargo => cargo.Category)
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .OrderBy(category => category);

    public IActionResult OnGet(int id)
    {
        var cargo = CargoStore.Cargoes.FirstOrDefault(item => item.Id == id);

        if (cargo is null)
        {
            return NotFound();
        }

        Cargo = cargo;
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existing = CargoStore.Cargoes.FirstOrDefault(item => item.Id == Cargo.Id);

        if (existing is null)
        {
            return NotFound();
        }

        existing.Name = Cargo.Name.Trim();
        existing.Category = Cargo.Category.Trim();
        existing.Quantity = Cargo.Quantity;
        existing.Location = Cargo.Location.Trim();
        existing.Weight = Cargo.Weight;
        existing.Description = Cargo.Description.Trim();
        existing.IsAvailable = Cargo.IsAvailable;

        TempData["SuccessMessage"] = $"Груз «{existing.Name}» успешно обновлён.";

        return RedirectToPage("./Index");
    }
}
