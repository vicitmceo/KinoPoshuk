# Архітектура системи — KinoPoshuk

Тиждень 2 (24.08–30.08): проєктування архітектури з урахуванням
[SRS](../../SRS.md).

> Ця версія документа замінює попередню після зауваження викладача:
> архітектура була занадто спрощеною (один `Program.cs` з усією логікою),
> а UML-діаграма не мала обґрунтування. Нижче — переробка на основі
> [прикладу Clean Architecture](https://gist.github.com/sunmeat/a3d8fc8da5231d261da088683c6ce40e),
> наданого викладачем, **реально реалізована в коді**, а не лише намальована.

## 1. Чому Clean Architecture, а не "один файл"

MVP-версія (тиждень 1) складалася з одного `Program.cs` (~27 рядків):
Minimal API endpoint напряму викликав `HttpClient` до OMDb і повертав сирий
JSON. Це працювало для однієї функції, але як тільки додався другий
use case (обране) і третій (історія пошуку) — стало зрозуміло, що:

- бізнес-правило "пошук записує запит в історію" нікуди було винести, крім
  як у той самий обробник запиту;
- перевірку дублікатів при додаванні в обране довелося б дублювати, якщо
  з'явиться ще один спосіб додавання (наприклад, імпорт списку);
- код, що знає формат відповіді OMDb, був би перемішаний з кодом, що знає
  про HTTP-статуси.

Тому проєкт розділено на 4 шари + SharedKernel — кожен шар відповідає за
одну категорію рішень і залежить лише від шарів "нижче" себе.

## 2. Структура проєкту (скріни реальної структури файлів)

### Бекенд — ASP.NET Core Web API (.NET 9), 4 шари Clean Architecture

![Структура бекенду](structure-backend.png)

**Presentation** (`Controllers/`, `Middleware/`, `Program.cs`, `wwwroot/`) —
HTTP-шар. **Application** (`Application/`) — сценарії використання,
бізнес-логіка, DTO. **Domain** (`Domain/`) — сутності та контракти
репозиторіїв, без залежностей від інших шарів. **Infrastructure**
(`Infrastructure/`) — доступ до SQL Server (EF Core) і зовнішнього OMDb API.
**SharedKernel** (`SharedKernel/`) — виключення, константи, розширення,
спільні для всіх шарів.

Напрямок залежностей: `Presentation → Application → Domain ← Infrastructure`
(Domain нічого не знає про інші шари — залежності інвертовано через
інтерфейси `IFavoriteMovieRepository`, `ISearchHistoryRepository`,
`IOmdbApiClient`, які реалізують класи з Infrastructure).

### Фронтенд — 3 екрани замість одного

![Структура фронтенду](structure-frontend.png)

На тижні 1 фронтенд мав лише форму пошуку. Зараз — три вкладки (`tabs.js`
керує перемиканням, кожна вкладка має власний JS-модуль, що працює з
окремим ендпоінтом):

- **Пошук** (`site.js` → `GET /api/search`)
- **Обране** (`favorites.js` → `GET/POST/DELETE /api/favorites`)
- **Історія** (`history.js` → `GET /api/history`)

### База даних — реалізовано (SQL Server LocalDB + EF Core)

![Структура БД](structure-database.png)

На відміну від попередньої версії документа (де БД лише планувалась), зараз
`KinoPoshukDbContext` підключено в `Program.cs`, міграція `InitialCreate`
застосовується автоматично при старті (`db.Database.Migrate()`), таблиці
`FavoriteMovies` і `SearchHistoryEntries` реально створюються і
використовуються ендпоінтами `/api/favorites` та `/api/history`.

## 3. UML-діаграма класів

Повна діаграма з детальним обґрунтуванням кожного класу і зв'язку —
[class-diagram.md](class-diagram.md). Діаграма відображає **фактичний код**
проєкту (18 класів/інтерфейсів), а не абстрактний ескіз.

## 4. Архітектурні та проєктні паттерни

| Паттерн | Де застосовано | Навіщо саме тут |
|---|---|---|
| **Layered / Clean Architecture** | Presentation → Application → Domain ← Infrastructure | Головна вимога зауваження викладача: розділити відповідальності на явні шари замість одного файлу |
| **Dependency Inversion + DI-контейнер** | `MoviesController` залежить від `IMovieService`, `MovieService` — від `IOmdbApiClient`/`IFavoriteMovieRepository`/`ISearchHistoryRepository` | Domain та Application не залежать від конкретних реалізацій (EF Core, HttpClient) — це дозволяє підмінити їх у тестах або замінити SQL Server на іншу БД без зміни бізнес-логіки |
| **Repository** | `Repository<T>` + похідні `FavoriteMovieRepository`, `SearchHistoryRepository` | Ізолює EF Core-специфічний код (`DbSet`, LINQ-запити) від сервісного шару; уникає дублювання спільних CRUD-операцій |
| **Adapter / Facade** | `OmdbApiClient` реалізує `IOmdbApiClient`, ховає формат `OmdbRawResponse` | Єдина точка, що знає про сирий формат OMDb API; заміна джерела даних не торкнеться інших класів |
| **DTO (Data Transfer Object)** | `MovieDto`, `FavoriteMovieDto`, `SearchHistoryDto` | Явний контракт між бекендом і фронтендом; захищає доменні сутності від прямої серіалізації в JSON |
| **Middleware pipeline (cross-cutting concerns)** | `ExceptionHandlingMiddleware`, `RequestLoggingMiddleware` | Обробка помилок і логування не належать бізнес-логіці — винесені в конвеєр ASP.NET Core, застосовуються до всіх ендпоінтів одразу |
| **Custom Exceptions (SharedKernel)** | `MovieNotFoundException`, `ExternalApiException` | Типізовані винятки замість `Exception` дозволяють `ExceptionHandlingMiddleware` повертати різні HTTP-статуси (404 проти 502) залежно від причини |

## 5. Потік даних (реалізовано)

```
Browser (index.html: вкладки Пошук / Обране / Історія)
   │  fetch GET /api/search?title=...
   ▼
MoviesController (Presentation)
   │  IMovieService.SearchAsync(title)
   ▼
MovieService (Application)
   │                              │
   │ IOmdbApiClient                │ ISearchHistoryRepository
   ▼                              ▼
OmdbApiClient (Infrastructure)    SearchHistoryRepository (Infrastructure)
   │  HttpClient → omdbapi.com        │  EF Core → SQL Server LocalDB
   ▼                                  ▼
MovieDto ◄── мапиться назад ──── KinoPoshukDbContext.SearchHistoryEntries
   │
   ▼
JSON-відповідь → Browser
```

Аналогічний потік для `/api/favorites` (GET/POST/DELETE) через
`IFavoriteMovieRepository` → `FavoriteMovieRepository` →
`KinoPoshukDbContext.FavoriteMovies`.

## 6. Що змінилось відносно SRS

SRS (тиждень 1) описував MVP без власної БД ("Out of Scope: збереження
історії/обраного"), а розділ 9 "План розвитку" планував це на наступну
ітерацію. Ця архітектура вже реалізує пункт 1 з того плану ("Перенесення
історії пошуків та обраного у власну БД") — тобто SRS буде оновлено разом
із цим документом, щоб пункт 9.1 перемістився з "заплановано" в
"реалізовано".
