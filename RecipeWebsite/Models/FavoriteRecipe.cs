using System.ComponentModel.DataAnnotations;

namespace RecipeWebsite.Models {
    public class FavoriteRecipe {

        /// <summary>
        /// Unique identifier for user who favorited the recipe
        /// </summary>
        [Required]
        public string? UserId { get; set; }

        /// <summary>
        /// Unique identifier for recipe from the recipe class
        /// </summary>
        [Required]
        public int RecipeId { get; set; }

    }
}
