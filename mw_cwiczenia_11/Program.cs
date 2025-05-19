using Microsoft.EntityFrameworkCore;
using mw_cwiczenia_11.Data;
using mw_cwiczenia_11.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<IDbService, DbService>();

builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
