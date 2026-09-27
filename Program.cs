using KinoPoshuk.BLL.Interfaces;
using KinoPoshuk.BLL.Services;
using KinoPoshuk.DAL.Data;
using KinoPoshuk.DAL.ExternalServices;
using KinoPoshuk.DAL.Interfaces;
using KinoPoshuk.DAL.Repositories;
using KinoPoshuk.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Render надає порт через змінну середовища PORT — слухаємо саме її,
// якщо вона задана (локально ж використовується стандартний launchSettings)
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

builder.Services.AddControllers();

// Render видає підключення до Postgres через змінну DATABASE_URL у форматі
// postgres://user:pass@host:port/db — Npgsql такий формат не розуміє напряму,
// тож конвертуємо його в keyword=value рядок підключення
var connectionString = builder.Configuration.GetConnectionString("KinoPoshukDb");
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2);
    var dbPort = uri.Port == -1 ? 5432 : uri.Port;
    connectionString = $"Host={uri.Host};Port={dbPort};Database={uri.AbsolutePath.TrimStart('/')};" +
                        $"Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}

builder.Services.AddDbContext<KinoPoshukDbContext>(options =>
    options.UseNpgsql(connectionString));

// фронтенд (React) хоститься на іншому домені (Netlify/Vercel) — потрібен CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddHttpClient<IOmdbApiClient, OmdbApiClient>();

builder.Services.AddScoped<IFavoriteMovieRepository, FavoriteMovieRepository>();
builder.Services.AddScoped<ISearchHistoryRepository, SearchHistoryRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<KinoPoshukDbContext>();
    db.Database.Migrate();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseCors("AllowFrontend");

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
