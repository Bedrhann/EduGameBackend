using EduGame.Data;
using EduGame.Service.Abstracts;
using EduGame.Service.Concretes;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. Servis Kayıtları (Dependency Injection)

// >>> EKSİK OLAN KISIM BURASI <<<
// GeminiAIService, HttpClient kullandığı için bu şekilde ekliyoruz:
builder.Services.AddHttpClient<IAIService, GeminiAIService>();

// GameContentManager servisi:
builder.Services.AddScoped<IGameContentService, GameContentManager>();

// UserManager servisi:
builder.Services.AddScoped<IUserService, UserManager>();

// 3. Controller ve Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ... Geri kalan kısım aynı ...
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();