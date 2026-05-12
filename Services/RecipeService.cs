using app_nutri.Data;
using app_nutri.Models;
using Microsoft.EntityFrameworkCore;

namespace app_nutri.Services;

public class RecipeService
{
    private readonly AppDbContext _context;

    public RecipeService(AppDbContext context) => _context = context;

    public async Task<List<Recipe>> GetAllAsync()
    {
        return await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .Include(r => r.Reviews)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Recipe?> GetByIdAsync(int id)
    {
        return await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .Include(r => r.Reviews)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddAsync(Recipe recipe, List<RecipeIngredient> ingredients)
    {
        recipe.CreatedAt = DateTime.Now;
        recipe.RecipeIngredients = ingredients;
        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Recipe recipe, List<RecipeIngredient> ingredients)
    {
        var existing = await _context.Recipes
            .Include(r => r.RecipeIngredients)
            .FirstOrDefaultAsync(r => r.Id == recipe.Id);

        if (existing is null) return;

        existing.Title = recipe.Title;
        existing.Description = recipe.Description;
        existing.Category = recipe.Category;
        existing.CuisineType = recipe.CuisineType;
        existing.ImageUrl = recipe.ImageUrl;
        existing.Servings = recipe.Servings;
        existing.AuthorName = recipe.AuthorName;

        _context.RecipeIngredients.RemoveRange(existing.RecipeIngredients);
        existing.RecipeIngredients = ingredients;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe is not null)
        {
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
        }
    }

    public double GetTotalCalories(Recipe recipe)
    {
        return recipe.RecipeIngredients.Sum(ri => ri.Quantity * (ri.Ingredient?.CaloriesPerUnit ?? 0));
    }

    public double GetCaloriesPerPerson(Recipe recipe)
    {
        return recipe.Servings > 0 ? GetTotalCalories(recipe) / recipe.Servings : 0;
    }
}
