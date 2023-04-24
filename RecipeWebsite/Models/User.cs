using Microsoft.AspNetCore.Identity;

namespace RecipeWebsite.Models {
    public class User : IdentityUser
    {
        public ICollection<Recipe> FavoriteRecipes { get; set; }

        public ICollection<Recipe> AuthoredRecipes { get; set; }
    }
}
