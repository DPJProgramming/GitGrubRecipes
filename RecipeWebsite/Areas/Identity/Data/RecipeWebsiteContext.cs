using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Models;

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
            //.HasForeignKey

        /*builder.Entity<User>()
            .HasMany<Recipe>(u => u.MyFavorites)*/

    }

    public DbSet<RecipeWebsite.Models.Recipe>? Recipe { get; set; }

    //public DbSet<User>? User { get; set; }

	public DbSet<Comment>? Comments { get; set; }

	/*public DbSet<MyFavorites>? myFavorites { get; set; }

    public DbSet<MyComments>? MyComments { get; set; }

    public DbSet<MyRecipes>? MyRecipes { get; set; }*/
}
