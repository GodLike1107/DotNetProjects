using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Data;
using ECommerceApp.ML;
using ECommerceApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("DefaultConnection not found in appsettings.json")));

// Add Identity with ApplicationUser
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<EmailService>(sp => new EmailService(builder.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Seed products and train recommendation model
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!db.Products.Any())
    {
        db.Products.AddRange(
            new Product { Name = "Laptop", Price = 999.99m, Category = "Electronics", Description = "High-performance laptop", ImageUrl = "" },
            new Product { Name = "Shirt", Price = 29.99m, Category = "Clothing", Description = "Casual shirt", ImageUrl = "" });
        db.SaveChanges();
    }

    var recommendationData = RecommendationDataLoader.LoadData(db);
    if (recommendationData.Any())
    {
        RecommendationModelTrainer.TrainAndSaveModel(recommendationData, "MLModel.zip");
    }
}

app.Run();