﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using RecipeWebsite.Data;
using RecipeWebsite.Models;
using System.Security.Claims;
using System.Text.Json;

namespace RecipeWebsite.Controllers {
    public class CommentController : Controller {

        private readonly RecipeWebsiteContext _context;
        private UserManager<User> _userManager;

        //class constructor
        public CommentController(RecipeWebsiteContext context, UserManager<User> userManager) {
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Creates a new comment object and inserts it into the database
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        // POST: CommentController/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JsonElement data) {

            //get current user and set a user object to that user specifically from the database
            User currentUser = await getCurrentUser();

            if(currentUser != null) { 
                User commentAuthor = await _context.Users.FindAsync(currentUser.Id);

                //set the comment text and passed in recipe
                string? commentText = data.GetProperty("commentText").GetString();
                
                if (string.IsNullOrWhiteSpace(commentText)) {
                    return Json(new { message = "Please enter a comment before submitting!" });
                }
                else {
                    Recipe? passedInRecipe = JsonSerializer.Deserialize<Recipe>(data.GetProperty("recipe").GetRawText());
                    Recipe? parentRecipe = await _context.Recipe.FirstOrDefaultAsync(r => r.RecipeId == passedInRecipe.RecipeId);

                    Comment newComment = new Comment {
                        ParentRecipe = parentRecipe,
                        CommentAuthor = commentAuthor,
                        Content = commentText,
                        Votes = 0
                    };

                    _context.Comments.Add(newComment);
                    _context.SaveChanges();

                    return Json(new { message = "Thanks for commenting!" });
                }
            }
            else {
                return Json(new { message = "Please register or log in to comment" });
            }
        }

        /// <summary>
        /// for future implementation. Will prepare fields for editing comment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: CommentController/Edit/5
        public ActionResult Edit(int id) {
            Comment comment = _context.Comments.Where(c => c.CommentId == id).SingleOrDefault();

            return View(comment);
        }

        /// <summary>
        /// For future implementation. Modifies specified comment in database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        // POST: CommentController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] JsonElement data) {

            int id = data.GetProperty("comment").GetInt32();
            string? text = data.GetProperty("commentText").GetString();

            Comment? comment = await _context.Comments.FindAsync(id);

            if (comment != null) {
                comment.Content = text;
                _context.SaveChanges();
                return Json(new { message = "Comment Edited Succesfully" });
            }
            else {
                return Json(new { message = "Comment does not exist" });
            }
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: CommentController/Delete/5
        public ActionResult Delete(int id) {
            return View();
        }

        /// <summary>
        /// Deletes comment from database
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        // POST: CommentController/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] JsonElement data) {
            string? authorId = data.GetProperty("author").GetString();
            User currentUser = await getCurrentUser();

            //make sure current user is also the comment author then delete comment from database
            if (currentUser != null && authorId == currentUser.Id) {
                int commentId = data.GetProperty("comment").GetInt32();
                Comment? commentToRemove = await _context.Comments.Where(c => c.CommentId == commentId).FirstOrDefaultAsync();
                _context.Comments.Remove(commentToRemove);
                _context.SaveChanges();

                return Json(new { message = "Comment Deleted" });
            }
            else {
                return Json(new { message = "Cannot delete other users comments" });
            }
        }

        /// <summary>
        /// Gets the currently logged in user
        /// </summary>
        /// <returns></returns>
        private async Task<User> getCurrentUser() {
            return await _userManager.GetUserAsync(User);
        }
    }
}
