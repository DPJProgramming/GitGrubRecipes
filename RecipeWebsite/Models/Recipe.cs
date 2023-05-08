using System.ComponentModel.DataAnnotations;

namespace RecipeWebsite.Models
{
    /// <summary>
    /// Represents a user's submitted recipe
    /// </summary>
    public class Recipe
    {
        /// <summary>
        /// Unique identifier for recipe
        /// </summary>
        [Key]
        public int RecipeId { get; set; }

        /// <summary>
        /// Name of the recipe
        /// </summary>
        [Required]
        public string? Title { get; set; }

        /// <summary>
        /// User who submitted the recipe
        /// </summary>
        public User Author { get; set; } = null!;

        /// <summary>
        /// URL that points at image
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// A list of ingredients stored as a string
        /// </summary>
        [Required]
        public ICollection<Ingredient> Ingredients { get; set; }

        /// <summary>
        /// Steps to prepare recipe stored as a string
        /// </summary>
        [Required]
        public string? Directions { get; set; }

        /// <summary>
        /// A list of Comment objects that stores attached comments
        /// </summary>
        public ICollection<Comment>? Comments { get; set; }

        public ICollection<User>? UsersFavorited { get; set; }
    }

    public class RecipeViewModel
    {
        public RecipeViewModel()
        {
            Ingredients = new List<Ingredient>();
        }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Directions { get; set; }

        public List<Ingredient> Ingredients { get; set; }
    }
}