using Microsoft.EntityFrameworkCore;
using SpacecraftMaintenanceStation.Application;
using SpacecraftMaintenanceStation.Infrastructure;
using SpacecraftMaintenanceStation.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; // Это настройка для запуска проекта на двух портах через CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      policy =>
//                      {
//                          policy.WithOrigins("https://localhost:7273",
//                                              "https://localhost:7165");
//                      });
//});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseBlazorFrameworkFiles(); //добавляем возможность раздавать файлы WASM самим сервером!
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Spacecraft Maintenance Station API v1");
    });
}

using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<StationDbContext>();
    await db.Database.MigrateAsync();

    await DbInitializer.InitializeAsync(db);
}

app.UseHttpsRedirection();

//app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html"); // раздача файла WASM

app.Run();
