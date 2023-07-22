using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        public string Title { get; set; }

        /// <summary>
        /// User who submitted the recipe
        /// </summary>
        public User? Author { get; set; } = null!;

        /// <summary>
        /// URL that points at image
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// A list of ingredients stored as a string
        /// </summary>
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        /// <summary>
        /// Steps to prepare recipe stored as a string
        /// </summary>
        [Required]
        public string Directions { get; set; }

        ///<summary>
        /// A description of the recipe that the user makes when creating the recipe
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A number from 1 to 5 calculated by averaging all votes from 
        /// users who have rated the recipe
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// Tracks the number of votes a recipe has
        /// Used for calculating a recipe's rating
        /// </summary>
        public int Votes { get; set; }

        /// <summary>
        /// The type of recipe i.e dessert, meat, side dish ect.
        /// </summary>
        public string Category { get; set; }
        
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
            RecipeId = 0;
        }

        public int RecipeId { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        public string Directions { get; set; }

        public List<Ingredient> Ingredients { get; set; }
    }

    public class RecipeDetailsViewModel
    {
        public Recipe Recipe { get; set; }
        public User? CurrentUser { get; set; } = null;
    }
}