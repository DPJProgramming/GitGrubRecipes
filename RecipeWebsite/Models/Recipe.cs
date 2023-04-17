using System.ComponentModel.DataAnnotations;

namespace RecipeWebsite.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }

        [Required]
        public string Title { get; set; }

        public int UserId { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string Ingredients { get; set; }

        [Required]
        public string Directions { get; set; }

        public List<Comment> Comments { get; set; }
    }
}