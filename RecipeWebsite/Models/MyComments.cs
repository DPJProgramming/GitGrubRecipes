using System.ComponentModel.DataAnnotations;

namespace RecipeWebsite.Models {
	public class MyComments {

		[Key]
		public int CommentId { get; set; }

		public User? CommentAuthor { get; set; }

		public Recipe? Recipe { get; set; }
	}
}
