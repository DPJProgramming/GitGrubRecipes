using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: CommentController
        public ActionResult Index() {
            return View();
        }

        // POST: CommentController/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JsonElement data) {

            //get current user and set a user object to that user specifically from the database
            User currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null) {
                User commentAuthor = await _context.Users.FindAsync(currentUser.Id);

                //set the comment text and passed in recipe
                string? commentText = data.GetProperty("commentText").GetString();
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
            else {
                return Json(new { message = "Please register or sign in to your account to comment" });
            }
        }

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id) {
            return View();
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection) {
            try {
                return RedirectToAction(nameof(Index));
            }
            catch {
                return View();
            }
        }

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id) {
            return View();
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection) {
            try {
                return RedirectToAction(nameof(Index));
            }
            catch {
                return View();
            }
        }
    }
}
