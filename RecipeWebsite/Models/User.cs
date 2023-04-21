using Microsoft.AspNetCore.Identity;
using System.Data;

namespace RecipeWebsite.Models {
    public class User : IdentityUser
    {
        public ICollection<Recipe> FavoriteRecipes { get; set; }
    }
}
