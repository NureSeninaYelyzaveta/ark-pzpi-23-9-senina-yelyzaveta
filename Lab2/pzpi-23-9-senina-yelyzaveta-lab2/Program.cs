using Microsoft.EntityFrameworkCore;
using WebApplicationArtProgress1.Data;
using WebApplicationArtProgress1.Services; 

var builder = WebApplication.CreateBuilder(args);

// додати контролери і налаштувати JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Swagger для документації API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// підключення DbContext до MySQL
builder.Services.AddDbContext<ArtProgressContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30)));
});

// реєстрація сервісів для DI
builder.Services.AddScoped<GradesService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<AdminService>();

var app = builder.Build();

// Swagger тільки для Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();  // редирект на HTTPS

app.UseAuthorization(); // авторизація

app.MapControllers();

app.Run();