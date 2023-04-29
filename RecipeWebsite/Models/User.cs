using Microsoft.AspNetCore.Identity;

namespace RecipeWebsite.Models {
    public class User : IdentityUser
    {
        /// <summary>
        /// Represents recipes that a user has 'favorited' 
        /// </summary>
        public ICollection<Recipe>? MyFavorites { get; set; }

        /// <summary>
        /// Represents comments that a user has made on a Recipe
        /// </summary>
		public ICollection<Comment>? MyComments { get; set; }

        /// <summary>
        /// Represents recipes that a user has created
        /// </summary>
        public ICollection<Recipe>? MyRecipes { get; set; } 
	}
}
