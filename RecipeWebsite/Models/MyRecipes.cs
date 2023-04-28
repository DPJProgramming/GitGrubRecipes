using System.ComponentModel.DataAnnotations;

namespace RecipeWebsite.Models {
	public class MyRecipes {

		[Key]
		public int MyRecipeId { get; set; }

		public User Author { get; set; } = null!;

		public Recipe Recipe { get; set; } = null!;
	}
}
