using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Data;
using RecipeWebsite.Models;

namespace RecipeWebsite.Controllers
{
    public class RecipeController : Controller
    {
        private readonly RecipeWebsiteContext _context;

        private readonly UserManager<User> _userManager;

        public RecipeController(RecipeWebsiteContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets a list of Recipe objects from the database
        /// </summary>
        /// <returns></returns>
        // GET: Recipe
        public async Task<IActionResult> Index()
        {
              return _context.Recipe != null ? 
                          View(await _context.Recipe.ToListAsync()) :
                          Problem("Entity set 'RecipeWebsiteContext.Recipe'  is null.");
        }

        /// <summary>
        /// Gets the details from the recipe that was clicked to display on view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Recipe/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .Include(r => r.Ingredients)
                .Include(r => r.Author)
                .Include(r => r.Comments)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            // sanitize directions to avoid XSS vulnerability
            recipe.Directions = Sanitize(recipe.Directions);

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                currentUser = _context.Users.Include(u => u.MyFavorites).FirstOrDefault(u => u.Id == currentUser.Id);
            }

            var viewModel = new RecipeDetailsViewModel
            {
                Recipe = recipe,
                CurrentUser = currentUser
            };

            return View(viewModel);
        }

        /// <summary>
        /// Gets the recipe fields ready for create view
        /// </summary>
        /// <returns></returns>
        // GET: Recipe/Create
        public IActionResult Create()
        {
            var viewModel = new RecipeViewModel();
            viewModel.Ingredients = new List<Ingredient>();
            return View(viewModel);
        }

        /// <summary>
        /// Creates a new recipe and inserts it into the database
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(RecipeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var recipe = new Recipe();
                recipe.Title = viewModel.Title;
                recipe.ImageUrl = !string.IsNullOrEmpty(viewModel.ImageUrl) ? viewModel.ImageUrl : "https://i.imgur.com/zIAshBo.png";
                recipe.Description = viewModel.Description;
                recipe.Category = viewModel.Category;
                recipe.SubCategory = ".";
                recipe.Directions = viewModel.Directions;

                // Assign Ingredients list from viewModel to Recipe
                recipe.Ingredients = viewModel.Ingredients.Select(i => { i.Recipe = recipe; return i; }).ToList();

                // Set the author to the currently logged in user
                recipe.Author = await _userManager.GetUserAsync(User);

                _context.Recipe.Add(recipe);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        /// <summary>
        /// Gets recipe fields from recipe ready for editing
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Recipe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Use Include() to load the related Ingredients
            var recipe = await _context.Recipe.Include(r => r.Ingredients).FirstOrDefaultAsync(r => r.RecipeId == id.Value);

            if (recipe == null)
            {
                return NotFound();
            }

            var viewModel = new RecipeViewModel();
            viewModel.RecipeId = recipe.RecipeId;
            viewModel.Title = recipe.Title;
            viewModel.ImageUrl = recipe.ImageUrl;
            viewModel.Directions = recipe.Directions;
            viewModel.Description = recipe.Description;
            viewModel.Ingredients = recipe.Ingredients?.ToList() ?? new List<Ingredient>(); // Null-check and initialization if null

            return View(viewModel);
        }

        /// <summary>
        /// Modifies recipe information for specified recipe in database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        // POST: Recipe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecipeViewModel viewModel)
        {
            if (id != viewModel.RecipeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid) // ModelState wants a Recipe object to be passed through a "Recipe field"
                                    // which is not possible. I've excluded it from the ModelState validation
            {
                try
                {
                    // Load the original recipe, including the ingredients
                    var originalRecipe = await _context.Recipe.Include(r => r.Ingredients).FirstOrDefaultAsync(r => r.RecipeId == id);

                    if (originalRecipe == null)
                    {
                        return NotFound();
                    }

                    // Update Recipe fields
                    originalRecipe.Title = viewModel.Title;
                    originalRecipe.ImageUrl = viewModel.ImageUrl;
                    originalRecipe.Category = viewModel.Category;
                    originalRecipe.SubCategory = "";
                    originalRecipe.Description = viewModel.Description;
                    originalRecipe.Directions = viewModel.Directions;

                    // Remove Ingredients
                    foreach (var ingredient in originalRecipe.Ingredients.ToList())
                    {
                        if (!viewModel.Ingredients.Any(i => i.IngredientId == ingredient.IngredientId))
                        {
                            _context.Entry(ingredient).State = EntityState.Deleted;
                        }
                    }

                    // Add and Update Ingredients
                    foreach (var viewModelIngredient in viewModel.Ingredients)
                    {
                        var originalIngredient = viewModelIngredient.IngredientId == 0 ? null :
                            originalRecipe.Ingredients.FirstOrDefault(i => i.IngredientId == viewModelIngredient.IngredientId);

                        if (originalIngredient == null)  // It's new
                        {
                            originalRecipe.Ingredients.Add(new Ingredient
                            {
                                Name = viewModelIngredient.Name,
                                Amount = viewModelIngredient.Amount,
                                RecipeId = originalRecipe.RecipeId,
                                Recipe = originalRecipe
                            });
                        }
                        else  // It exists
                        {
                            originalIngredient.Name = viewModelIngredient.Name;
                            originalIngredient.Amount = viewModelIngredient.Amount;
                            originalIngredient.RecipeId = originalRecipe.RecipeId;
                            originalIngredient.Recipe = originalRecipe;
                        }
                    }


                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(viewModel.RecipeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(viewModel);
        }

        /// <summary>
        /// Gets the delete view for recipe
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Recipe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        /// <summary>
        ///Removes recipe from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: Recipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recipe == null)
            {
                return Problem("Entity set 'RecipeWebsiteContext.Recipe'  is null.");
            }

            // Find the recipe
            var recipe = await _context.Recipe
                .Include(r => r.UsersFavorited)  // Include the users who have favorited this recipe
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            if (recipe != null)
            {
                // Remove all FavoriteRecipes entries associated with this recipe
                foreach (var user in recipe.UsersFavorited)
                {
                    var favoriteRecipe = await _context.FavoriteRecipes
                        .FirstOrDefaultAsync(fr => fr.RecipeId == id && fr.UserId == user.Id);

                    if (favoriteRecipe != null)
                    {
                        _context.FavoriteRecipes.Remove(favoriteRecipe);
                    }
                }

                // Now remove the recipe itself
                _context.Recipe.Remove(recipe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Gets a list of favorite objects from the database
        /// </summary>
        /// <returns></returns>
        //Get: FavoriteRecipes
        [Authorize]
        public async Task<IActionResult> MyFavorites() {

            //gets logged in user 
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;

            //gets the id of the current user
            string? currentUserID = this.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //make a list FavoriteRecipe wher id matches currently logged in user
            List<FavoriteRecipe> favoriteRecipeModelList = await (_context.FavoriteRecipes.Where(f => f.UserId == currentUserID).ToListAsync());

            //create a list to hold recipes
            List<Recipe> recipeModelList = new List<Recipe>();

            //add favoriteRecipe list items to recipe list where the recipeid matches
            foreach(FavoriteRecipe r in favoriteRecipeModelList) {
                Recipe? recipeToAdd = _context.Recipe?.Where(v => v.RecipeId == r.RecipeId).FirstOrDefault();
                recipeModelList.Add(recipeToAdd);
            }


            return _context.FavoriteRecipes != null ?
                        View(recipeModelList) :
                        Problem("Entity set 'RecipeWebsiteContext.FavoriteRecipes'  is null.");
        }

        /// <summary>
        /// Adds or removes the favorite recipe for user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //adds the recipe to a users favorite recipes aka add entry into the FavoriteRecipes table
        [HttpPost]
        public IActionResult ToggleFavorite([FromBody] int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var existingFavorite = _context.FavoriteRecipes.FirstOrDefault(fr => fr.RecipeId == id && fr.UserId == userId);

            if (existingFavorite != null)
            {
                _context.FavoriteRecipes.Remove(existingFavorite);
                _context.SaveChanges();
                return Json(new { message = "Removed from favorites" });
            }
            else
            {
                var favorite = new FavoriteRecipe
                {
                    RecipeId = id,
                    UserId = userId
                };
                _context.FavoriteRecipes.Add(favorite);
                _context.SaveChanges();
                return Json(new { message = "Added to favorites" });
            }
        }

        /// <summary>
        /// Checks if the specified recipe exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // Returns true if the recipe exists in the database
        private bool RecipeExists(int id)
        {
          return _context.Recipe.Any(e => e.RecipeId == id);
        }

        // Sanitizes a string to be displayed as Raw HTML to avoid cross-site scripting (XSS) vulnerability
        private static string Sanitize(string str)
        {
            var sanitizer = new HtmlSanitizer();
            var sanitizedString = sanitizer.Sanitize(str.Replace("\r\n", "<br />"));
            return sanitizedString;
        }

        public IActionResult test() {
            return View();
        }
    }
}
