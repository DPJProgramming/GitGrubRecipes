using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ValidateNever]
        public Recipe Recipe { get; set; }
    }
}
