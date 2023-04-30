using System.ComponentModel.DataAnnotations;

namespace RecipeWebsite.Models
{
    public class Ingredient
    {
        /// <summary>
        /// unique identifier for ingredient
        /// </summary>
        public int IngredientId { get; set; }
        /// <summary>
        /// name of ingredient
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// amount of ingredient needed
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// used to connect recipe to necessary ingredients
        /// </summary>
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
