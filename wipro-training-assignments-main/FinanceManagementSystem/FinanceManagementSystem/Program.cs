using FinanceManagementSystem.DAO;
using FinanceManagementSystem.Data;  
using FinanceManagementSystem.Filters;
using FinanceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(o =>
{
    o.Filters.Add<SessionLayoutFilter>();
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// 🔗 Register DbContext with connection string "FinanceContext"
builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FinanceContext")));

builder.Services.AddScoped<IFinanceRepository, FinanceRepositoryImpl>();
// Build the app
var app = builder.Build();

// Seed default categories if none exist
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
    if (!db.ExpenseCategories.Any())
    {
        db.ExpenseCategories.AddRange(
            new ExpenseCategory { CategoryName = "Food" },
            new ExpenseCategory { CategoryName = "Transportation" },
            new ExpenseCategory { CategoryName = "Utilities" },
            new ExpenseCategory { CategoryName = "Entertainment" },
            new ExpenseCategory { CategoryName = "Other" }
        );
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();