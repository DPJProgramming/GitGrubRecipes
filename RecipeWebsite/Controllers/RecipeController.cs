using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        // GET: Recipe
        public async Task<IActionResult> Index()
        {
              return _context.Recipe != null ? 
                          View(await _context.Recipe.ToListAsync()) :
                          Problem("Entity set 'RecipeWebsiteContext.Recipe'  is null.");
        }

        // GET: Recipe/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Recipe/Create
        public IActionResult Create()
        {
            var viewModel = new RecipeViewModel();
            viewModel.Ingredients = new List<Ingredient>();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecipeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var recipe = new Recipe();
                recipe.Title = viewModel.Title;
                recipe.ImageUrl = viewModel.ImageUrl;
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
            viewModel.Ingredients = recipe.Ingredients?.ToList() ?? new List<Ingredient>(); // Null-check and initialization if null

            return View(viewModel);
        }

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
                    foreach (var ingredientViewModel in viewModel.Ingredients)
                    {
                        var originalIngredient = ingredientViewModel.IngredientId == 0 ? null :
                            originalRecipe.Ingredients.FirstOrDefault(i => i.IngredientId == ingredientViewModel.IngredientId);

                        if (originalIngredient == null)  // It's new
                        {
                            originalRecipe.Ingredients.Add(new Ingredient
                            {
                                Name = ingredientViewModel.Name,
                                Amount = ingredientViewModel.Amount,
                                RecipeId = originalRecipe.RecipeId,
                                Recipe = originalRecipe
                            });
                        }
                        else  // It exists
                        {
                            originalIngredient.Name = ingredientViewModel.Name;
                            originalIngredient.Amount = ingredientViewModel.Amount;
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

        // POST: Recipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recipe == null)
            {
                return Problem("Entity set 'RecipeWebsiteContext.Recipe'  is null.");
            }
            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipe.Remove(recipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Get: FavoriteRecipes
        public async Task<IActionResult> MyFavorites() {

            //gets logged in user 
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;

            //gets the id of the current user
            string? currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            //make a list from the table of entries matching currently logged in user
            List<FavoriteRecipe> myFavorites = await (_context.FavoriteRecipes.Where(f => f.UserId == currentUserID).ToListAsync());

            return _context.FavoriteRecipes != null ?
                        View(myFavorites) :
                        Problem("Entity set 'RecipeWebsiteContext.FavoriteRecipes'  is null.");
        }

        private bool RecipeExists(int id)
        {
          return _context.Recipe.Any(e => e.RecipeId == id);
        }
    }
}
