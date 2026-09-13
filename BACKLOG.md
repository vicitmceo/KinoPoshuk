# Product Backlog & Sprint Planning — KinoPoshuk

Методологія: **Scrum**, тривалість спринту — 1 тиждень. Ролі — див. [TEAM.md](TEAM.md).
Канбан-дошка: [GitHub Projects](https://github.com/users/vicitmceo/projects/1).

## Product Backlog (пріоритизація MoSCoW)

| # | Задача | Пріоритет | Статус |
|---|---|---|---|
| 1 | SRS документ (функціональні/нефункціональні вимоги, use cases) | Must | ✅ Done (Sprint 1) |
| 2 | Clean Architecture (Presentation/Application/Domain/Infrastructure/SharedKernel) | Must | ✅ Done (Sprint 1) |
| 3 | Репозиторії (`IRepository<T>`, `IFavoriteMovieRepository`, `ISearchHistoryRepository`) + EF Core/SQL Server | Must | ✅ Done (Sprint 1) |
| 4 | UML-діаграма класів з обґрунтуванням | Must | ✅ Done (Sprint 1) |
| 5 | Unit-тести для `MovieService` (NUnit + Moq) | Should | ✅ Done (Sprint 2) |
| 6 | Кешування відповідей OMDb API (зменшення зовнішніх запитів) | Should | 🔲 To Do |
| 7 | Реєстрація/авторизація користувачів, персональне "Обране" | Could | 🔲 To Do |
| 8 | Пагінація та розширений пошук (за роком, жанром) | Could | 🔲 To Do |
| 9 | Адаптивна верстка під мобільні пристрої | Could | 🔲 To Do |
| 10 | CI (GitHub Actions: build + test на кожен push) | Should | 🔲 To Do |
| 11 | Деплой застосунку (Azure/Render) | Must | 🔲 To Do |

## Sprint 1 (17.08 – 30.08) — завершено

Мета: MVP + перехід на Clean Architecture з реальною БД.
Результат: задачі 1–4 з Product Backlog виконано (SRS, архітектура, репозиторії, UML).

## Sprint 2 (31.08 – 06.09, дедлайн здачі 08.09) — поточний

**Мета спринту:** ввести практику Scrum (ролі, беклог, канбан) і зробити перший крок до тестового покриття.

**Sprint Backlog (задачі цього тижня):**

| Задача | Опис | Статус |
|---|---|---|
| Впровадження SCRUM-ролей | Явний розподіл Product Owner / Scrum Master / Development Team в [TEAM.md](TEAM.md) | ✅ Done |
| Канбан-дошка | Створення GitHub Projects дошки з колонками Backlog/To Do/In Progress/Done, перенесення задач з Product Backlog | ✅ Done |
| Ведення Product Backlog | Цей файл — структурований список задач з пріоритетами MoSCoW | ✅ Done |
| Unit-тести `MovieService` | Проєкт `KinoPoshuk.Tests` (NUnit + Moq): покрито `SearchAsync`, `AddFavoriteAsync`, `RemoveFavoriteAsync`, `GetHistoryAsync` — 7 тестів, усі проходять | ✅ Done |
| Планування Sprint 3 | Наступний спринт — див. нижче | ✅ Done |

## Sprint 3 (07.09 – 13.09) — заплановано

Кандидати з Product Backlog (буде уточнено на плануванні): кешування OMDb (#6), CI через GitHub Actions (#10) — обидва не потребують великих архітектурних змін і логічно продовжують тему "інструменти й якість коду", розпочату тестами в Sprint 2.
