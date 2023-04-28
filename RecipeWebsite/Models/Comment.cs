using System.ComponentModel.DataAnnotations;

namespace RecipeWebsite.Models
{
    /// <summary>
    /// Represents a user's submitted commment
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Unique identifier for comment
        /// </summary>
        [Key]
        public int CommentId { get; set; }

        /// <summary>
        /// Unique identifier for recipe
        /// NOTE: Since comments are stored in recipe objects, the recipe ID might be redundant.
        /// </summary>
        public Recipe ParentRecipe { get; set; } = null!;
        
        /// <summary>
        /// Unique identifier for user that submitted the comment
        /// </summary>
        public User CommentAuthor { get; set; } = null!;

        /// <summary>
        /// The text content of the submitted comment
        /// </summary>
        [Required]
        public string? Content { get; set; }

        /// <summary>
        /// An integer that stores the number of votes/likes a comment has
        /// </summary>
        public int Votes { get; set; }
    }
}
