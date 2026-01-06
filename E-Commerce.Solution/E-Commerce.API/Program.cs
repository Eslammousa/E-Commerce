using E_Commerce.API.Middlewares;
using E_Commerce.Core.Domain.RepositoryContracts;
using E_Commerce.Core.Mapping;
using E_Commerce.Core.Services;
using E_Commerce.Core.ServicesContracts;
using E_Commerce.Infrastructure.Data.DBContext;
using E_Commerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString =
    builder.Configuration.GetConnectionString("constr")
    ?? throw new InvalidOperationException("Connection string 'constr' not found!");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ICategoryRepository , CategoryRepository>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandlingMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
