using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Data;
using RecipeWebsite.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("GitGrubRecipesConnection") ?? throw new InvalidOperationException("Connection string 'GitGrubRecipesConnection' not found.");

builder.Services.AddDbContext<RecipeWebsiteContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<RecipeWebsiteContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//can configure password requirements here
builder.Services.Configure<IdentityOptions>(options => {

    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

var app = builder.Build();

using IServiceScope scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<RecipeWebsiteContext>();
db.Database.Migrate();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    //changed this from Home to Recipe as the first page that loads
    pattern: "{controller=Recipe}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

public static partial class Program {
    public static string apiKey() {
        var builder = WebApplication.CreateBuilder();
        return builder.Configuration.GetSection("NutritionApi").Value;
    }
}



