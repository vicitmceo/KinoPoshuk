using KinoPoshuk.Application.Interfaces;
using KinoPoshuk.Application.Services;
using KinoPoshuk.Domain.Interfaces;
using KinoPoshuk.Infrastructure.Data;
using KinoPoshuk.Infrastructure.ExternalServices;
using KinoPoshuk.Infrastructure.Repositories;
using KinoPoshuk.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<KinoPoshukDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KinoPoshukDb")));

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

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
