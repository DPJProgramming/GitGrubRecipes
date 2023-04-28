using Microsoft.AspNetCore.Identity;

namespace RecipeWebsite.Models {
    public class User : IdentityUser
    {
        /// <summary>
        /// A list/table of recipes that a user has 'favorited' 
        /// </summary>
        public ICollection<Recipe>? MyFavorites { get; set; }

        /// <summary>
        /// A list/table of comments that a user has made on a recipe
        /// </summary>
		public ICollection<Comment>? MyComments { get; set; }

        /// <summary>
        /// a list/table of recipes that a user has created
        /// </summary>
        public ICollection<Recipe>? MyRecipes { get; set; } 
	}
}
