using System;
using System.Collections.Generic;
using System.Linq;
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
            // Check for the necessary properties in the view model
            // ModelState was giving me a headache
            if (!string.IsNullOrWhiteSpace(viewModel.Title) && !string.IsNullOrWhiteSpace(viewModel.Directions))
            {
                var recipe = new Recipe();
                recipe.Title = viewModel.Title;
                recipe.ImageUrl = viewModel.ImageUrl;
                recipe.Directions = viewModel.Directions;

                // Create a new list of Ingredient objects and copy the values from the view model
                ICollection<Ingredient> ingredients = new List<Ingredient>();
                foreach (Ingredient ingredient in viewModel.Ingredients)
                {
                    ingredients.Add(new Ingredient
                    {
                        Name = ingredient.Name,
                        Amount = ingredient.Amount,
                        Recipe = recipe // Set the Recipe property for each Ingredient
                    });
                }
                recipe.Ingredients = ingredients;

                // Set the author to the currently logged in user
                recipe.Author = await _userManager.GetUserAsync(User);

                _context.Recipe.Add(recipe);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Recipe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        // POST: Recipe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeId,Title,UserId,ImageUrl,Ingredients,Directions")] Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.RecipeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
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

        private bool RecipeExists(int id)
        {
          return (_context.Recipe?.Any(e => e.RecipeId == id)).GetValueOrDefault();
        }
    }
}
