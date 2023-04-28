using System.ComponentModel.DataAnnotations;

namespace RecipeWebsite.Models {
	public class MyFavorites {

		[Key]
		public int FavoriteId { get; set; }

		public User? User { get; set; }

		public Recipe Recipe { get; set; } = null!;
	}
}
