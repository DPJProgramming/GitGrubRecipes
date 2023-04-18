using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Models;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;

namespace RecipeWebsite.Data;

public class RecipeWebsiteContext : IdentityDbContext<User>
{
    public RecipeWebsiteContext(DbContextOptions<RecipeWebsiteContext> options)
        : base(options)
    {
    }

    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<User>()
            .HasMany(u => u.FavoriteRecipes)
            .WithMany(r => r.UsersFavorited)
            .UsingEntity(join => join.ToTable("Favorites"));
    }
}
