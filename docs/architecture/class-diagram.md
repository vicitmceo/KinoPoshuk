# UML-діаграма класів — KinoPoshuk

Діаграма описує основні сутності поточної (MVP) та запланованої (наступна
ітерація за [SRS](../../SRS.md), розділ 9 "План розвитку") архітектури:
сервіс пошуку, клієнт зовнішнього OMDb API, DTO фільму та заплановані
сутності бази даних (обрані фільми, історія пошуку).

```mermaid
classDiagram
    class MovieSearchService {
        -IOmdbClient omdbClient
        -AppDbContext db
        +SearchAsync(title: string) MovieDto
    }

    class IOmdbClient {
        <<interface>>
        +GetByTitleAsync(title: string) MovieDto
    }

    class OmdbClient {
        -string apiKey
        -HttpClient httpClient
        +GetByTitleAsync(title: string) MovieDto
    }

    class MovieDto {
        +string Title
        +int Year
        +string Genre
        +string Director
        +string Actors
        +string Plot
        +string PosterUrl
        +string ImdbRating
        +string Runtime
        +bool Response
    }

    class AppDbContext {
        <<заплановано>>
        +DbSet~FavoriteMovie~ Favorites
        +DbSet~SearchHistoryEntry~ History
    }

    class FavoriteMovie {
        <<заплановано>>
        +int Id
        +string ImdbId
        +string Title
        +int Year
        +string PosterUrl
        +DateTime AddedAt
    }

    class SearchHistoryEntry {
        <<заплановано>>
        +int Id
        +string Query
        +DateTime SearchedAt
        +bool WasFound
    }

    MovieSearchService --> IOmdbClient : використовує
    OmdbClient ..|> IOmdbClient : реалізує
    MovieSearchService --> MovieDto : повертає
    MovieSearchService --> AppDbContext : веде історію (заплановано)
    AppDbContext "1" --> "*" FavoriteMovie : Favorites
    AppDbContext "1" --> "*" SearchHistoryEntry : History
    FavoriteMovie ..> MovieDto : створюється з даних пошуку
```

## Пояснення зв'язків

- **`MovieSearchService` → `IOmdbClient`** — сервіс пошуку залежить від
  абстракції клієнта зовнішнього API, а не від конкретної реалізації
  (Dependency Inversion)
- **`OmdbClient` реалізує `IOmdbClient`** — конкретна реалізація на базі
  `HttpClient`, інкапсулює формування URL та API-ключ OMDb
- **`MovieSearchService` → `MovieDto`** — результат пошуку повертається як
  типізований DTO, а не сирий JSON
- **`AppDbContext` 1 → * `FavoriteMovie` / `SearchHistoryEntry`** —
  заплановані сутності для збереження обраних фільмів та історії пошуків
  (наступна ітерація, потребує SQL Server + EF Core)
