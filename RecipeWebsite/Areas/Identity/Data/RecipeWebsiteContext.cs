using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Models;
using System.Reflection.Emit;

namespace RecipeWebsite.Data;

public class RecipeWebsiteContext : IdentityDbContext<User>
{
    public RecipeWebsiteContext(DbContextOptions<RecipeWebsiteContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        
        builder.Entity<Recipe>()
            .HasOne<User>(r => r.Author)
            .WithMany(u => u.MyRecipes);

        builder.Entity<Recipe>()
            .HasMany(r => r.Ingredients)
            .WithOne(i => i.Recipe)
            .HasForeignKey(i => i.RecipeId);

        builder.Entity<Recipe>()
        .HasMany(r => r.UsersFavorited)
        .WithMany(u => u.MyFavorites)
        .UsingEntity<FavoriteRecipe>(
            x => x.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade),
            x => x.HasOne<Recipe>().WithMany().HasForeignKey("RecipeId").OnDelete(DeleteBehavior.NoAction),
            x =>
            {
                x.HasKey("UserId", "RecipeId");
                x.ToTable("FavoriteRecipes");
            });

        //previous code before adding FavoriteRecipe
       /* builder.Entity<Recipe>()
        .HasMany(r => r.UsersFavorited)
        .WithMany(u => u.MyFavorites)
        .UsingEntity<Dictionary<string, object>>(
            "FavoriteRecipe",
            x => x.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade),
            x => x.HasOne<Recipe>().WithMany().HasForeignKey("RecipeId").OnDelete(DeleteBehavior.NoAction),
            x => {
                x.HasKey("UserId", "RecipeId");
                x.ToTable("FavoriteRecipes");
            }) ;*/


        //attempt at configuring FavoriteRecipe could delete in future
        /*builder.SharedTypeEntity<Dictionary<string, object>>(
            "FavoriteRecipes", fr => {
                fr.Property<string>("UserId");
                fr.Property<int>("RecipeId");
            });*/
    }

    public DbSet<RecipeWebsite.Models.Recipe>? Recipe { get; set; }

    public DbSet<Comment>? Comments { get; set; }

    public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }
}
