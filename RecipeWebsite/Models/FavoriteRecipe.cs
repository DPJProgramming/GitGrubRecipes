using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeWebsite.Models {

    [Table("FavoriteRecipes")]
    public class FavoriteRecipe {

        /// <summary>
        /// Unique identifier for user who favorited the recipe
        /// </summary>
        [Key]
        [Required]
        public string? UserId { get; set; }

        /// <summary>
        /// Unique identifier for recipe from the recipe class
        /// </summary>
        [Key]
        [Required]
        public int RecipeId { get; set; }

    }
}
