# WT-5 Razor Pages

Учебное веб-приложение **CargoFlow** для управления складскими грузами, выполненное на
ASP.NET Core Razor Pages в рамках лабораторной работы WT-5.

## Репозиторий

https://github.com/Balur77/wt-lab5-5razor-yushkevich

## Опубликованное приложение

Публичный URL Render: **будет добавлен после выполнения deployment**
(формат: `https://wt-lab5-5razor-yushkevich.onrender.com`).

## Описание предметной области

CargoFlow автоматизирует базовый учёт грузов на складе. Основная сущность — `Cargo`
(груз), перенесённая из лабораторной работы WT-4. Для каждой позиции хранятся:

- идентификатор;
- название;
- категория;
- количество;
- место хранения;
- вес одной единицы;
- описание;
- признак доступности на складе.

Общий вес партии и текстовый статус вычисляются в модели.

## Функциональность

- главная страница с показателями склада и последними добавленными грузами;
- каталог грузов в адаптивных карточках Bootstrap;
- поиск по названию, месту хранения и описанию;
- фильтрация по категории;
- страница подробной информации о грузе;
- добавление груза через форму с серверной и клиентской валидацией;
- редактирование груза;
- удаление груза с подтверждением;
- единый интерфейс и навигация на основе `_Layout.cshtml`;
- публикация в Docker-контейнере с поддержкой переменной окружения `PORT`.

Данные хранятся в памяти приложения в статическом классе `CargoStore`.
После перезапуска приложения добавленные пользователем записи исчезают — это
нормальное ограничение лабораторной работы, база данных не используется.

## Технологии

- C# 13 / .NET 9;
- ASP.NET Core Razor Pages;
- Razor Tag Helpers;
- Bootstrap 5 (локальные статические файлы);
- jQuery Validation / Unobtrusive Validation;
- Docker;
- Render Web Service.

## Структура проекта

```text
wt-lab5-5razor-yushkevich/
├── Data/
│   └── CargoStore.cs           # единое хранилище в памяти
├── Models/
│   └── Cargo.cs                # модель предметной области
├── Pages/
│   ├── Cargo/
│   │   ├── Index.cshtml        # каталог
│   │   ├── Index.cshtml.cs     # OnGet(), поиск и фильтрация
│   │   ├── Create.cshtml       # форма добавления
│   │   ├── Create.cshtml.cs    # OnGet(), OnPost(), [BindProperty]
│   │   ├── Details.cshtml      # подробная карточка
│   │   ├── Details.cshtml.cs   # OnGet(int id), NotFound()
│   │   ├── Edit.cshtml         # форма редактирования
│   │   ├── Edit.cshtml.cs      # OnGet(int id), OnPost()
│   │   ├── Delete.cshtml       # подтверждение удаления
│   │   └── Delete.cshtml.cs    # OnPost(int id)
│   ├── Shared/
│   │   ├── _CargoCard.cshtml   # переиспользуемая карточка груза
│   │   ├── _Layout.cshtml      # общий layout и Bootstrap Navbar
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── Index.cshtml            # главная страница
│   ├── Index.cshtml.cs         # статистика для главной
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
├── wwwroot/
│   ├── css/site.css            # тема, hover-эффекты, адаптивность
│   ├── images/warehouse-hero.svg
│   ├── js/site.js
│   └── lib/                    # Bootstrap, jQuery, валидация
├── docs/screenshots/           # скриншоты приложения
├── Dockerfile
├── .dockerignore
├── .gitignore
├── Program.cs
├── README.md
└── wt-lab5-5razor-yushkevich.csproj
```

## Модель и PageModel

`Cargo` содержит атрибуты `[Required]`, `[StringLength]`, `[Range]` и `[Display]`.
Валидация выполняется как на клиенте (jQuery Validation), так и на сервере через
`ModelState.IsValid`.

В `CreateModel` и `EditModel` используется `[BindProperty]`, поэтому данные формы
автоматически связываются со свойством `Cargo`:

```csharp
[BindProperty]
public Cargo Cargo { get; set; } = new();

public IActionResult OnPost()
{
    if (!ModelState.IsValid)
    {
        return Page();
    }

    // сохранение в CargoStore
    return RedirectToPage("./Index");
}
```

Основные обработчики:

- `OnGet()` — загружает список из `CargoStore` для страницы каталога;
- `OnGet(int id)` — находит груз по идентификатору или возвращает `NotFound()`;
- `OnPost()` — проверяет модель, добавляет или обновляет запись;
- `OnPost(int id)` — удаляет запись и перенаправляет в каталог.

## Запуск локально

Требуется .NET 9 SDK.

```bash
dotnet restore
dotnet run
```

Приложение откроется по адресу `http://localhost:10000`.

Основные маршруты:

- `/` — главная;
- `/Cargo/Index` (или `/Cargo`) — каталог;
- `/Cargo/Create` — добавление;
- `/Cargo/Details?id=1` — подробная карточка;
- `/Cargo/Edit?id=1` — редактирование;
- `/Cargo/Delete?id=1` — удаление.

## Docker

Сборка образа:

```bash
docker build -t wt-lab5-5razor-yushkevich .
```

Запуск контейнера:

```bash
docker run --rm -p 8080:8080 -e PORT=8080 wt-lab5-5razor-yushkevich
```

Приложение будет доступно по адресу `http://localhost:8080`.
`Program.cs` читает переменную окружения `PORT` и запускает сервер на
`http://0.0.0.0:{port}`, как требуется для Render.

## Публикация на Render

1. Создать **New → Web Service**.
2. Подключить репозиторий `Balur77/wt-lab5-5razor-yushkevich`.
3. Указать:
   - Runtime: **Docker**;
   - Branch: **main**;
   - Dockerfile Path: `./Dockerfile`.
4. Запустить deployment и дождаться статуса **Live**.
5. Проверить маршруты `/`, `/Cargo/Index`, `/Cargo/Create`, `/Cargo/Details?id=1`.
6. Вписать публичный URL в раздел «Опубликованное приложение» этого README.

Render автоматически передаёт переменную `PORT`, поэтому приложение слушает
внешний порт без дополнительной настройки.

## Скриншоты

| Страница | Файл |
| --- | --- |
| Главная | [`docs/screenshots/01-home.png`](docs/screenshots/01-home.png) |
| Каталог | [`docs/screenshots/02-catalog.png`](docs/screenshots/02-catalog.png) |
| Добавление | [`docs/screenshots/03-create.png`](docs/screenshots/03-create.png) |
| Подробная карточка | [`docs/screenshots/04-details.png`](docs/screenshots/04-details.png) |
| Редактирование | [`docs/screenshots/05-edit.png`](docs/screenshots/05-edit.png) |
| Удаление | [`docs/screenshots/06-delete.png`](docs/screenshots/06-delete.png) |
| Результат добавления | [`docs/screenshots/07-create-success.png`](docs/screenshots/07-create-success.png) |
| Мобильная версия | [`docs/screenshots/09-mobile-catalog.png`](docs/screenshots/09-mobile-catalog.png) |

## Сравнение MVC и Razor Pages

| ASP.NET Core MVC | Razor Pages |
| --- | --- |
| Controller | PageModel |
| Action | Handler (`OnGet`, `OnPost`) |
| View | Razor Page (`.cshtml`) |
| `Views/...` | `Pages/...` |
| `Index()` | `OnGet()` |
| `[HttpPost]` + action | `OnPost()` |
| `RedirectToAction()` | `RedirectToPage()` |

Razor Pages не является отдельным серверным фреймворком: это модель организации
приложений ASP.NET Core, которая использует Razor, middleware, dependency injection,
маршрутизацию и Tag Helpers общей инфраструктуры ASP.NET Core. Код страницы и её
обработчики находятся рядом, поэтому для небольших сценарных страниц Razor Pages
получаются компактнее, чем связка Controller + View в MVC.

## Вывод

В ходе работы приложение WT-4 было переведено с MVC на Razor Pages. Освоены
`PageModel`, обработчики `OnGet()`/`OnPost()`, привязка модели через
`[BindProperty]`, валидация, Tag Helpers, единый layout и адаптивное
Bootstrap-оформление. Приложение подготовлено к публикации: добавлены Dockerfile,
поддержка `PORT` и инструкция для Render.
