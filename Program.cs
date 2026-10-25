using System.Globalization;

// Десятичным разделителем в HTML-формах является точка, поэтому
// для модели связывания используем русскую культуру с точкой.
var appCulture = (CultureInfo)CultureInfo.GetCultureInfo("ru-RU").Clone();
appCulture.NumberFormat.NumberDecimalSeparator = ".";
appCulture.NumberFormat.CurrencyDecimalSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = appCulture;
CultureInfo.DefaultThreadCurrentUICulture = appCulture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

// Render передаёт внешний порт через переменную окружения PORT.
// Приложение должно слушать все сетевые интерфейсы внутри контейнера.
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";

app.Run($"http://0.0.0.0:{port}");
