﻿using System.ComponentModel.DataAnnotations;

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
        /// Unique identifier for user that submitted the comment
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The text content of the submitted comment
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// An integer that stores the number of votes/likes a comment has
        /// </summary>
        public int Votes { get; set; }
    }
}
