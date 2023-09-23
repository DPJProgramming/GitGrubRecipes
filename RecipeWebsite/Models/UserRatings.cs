using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeWebsite.Models {
    public class UserRatings {

        /// <summary>
        /// An identifier for a UserRating
        /// </summary>
        [Key]
        public int ratingId { get; set; }

        /// <summary>
        /// The recipe that a specific rating/user combination belongs to
        /// </summary>
        [ForeignKey("RecipeId")]
        public int RecipeId { get; set; }

        /// <summary>
        /// A user identified by their id that has rated a specific recipe
        /// </summary>
        public string? UserRated { get; set; }

        /// <summary>
        /// A number value a user assigns to a recipe to represent the quality of the recipe
        /// </summary>
        public int Rating { get; set; }
    }
}
