using EduGame.Data;
using Microsoft.EntityFrameworkCore;
using EduGame.Service.Abstracts;
using EduGame.Service.Concretes;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Servisini Ekle
// appsettings.json dosyasından "DefaultConnection" kısmını okur.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. Controller ve Swagger Servisleri
builder.Services.AddScoped<IGameContentService, GameContentManager>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Middleware (İstek Hattı)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();