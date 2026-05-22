using app_nutri.Data;
using app_nutri.Models;
using Microsoft.EntityFrameworkCore;

namespace app_nutri.Services;

public class RecipeService
{
    private readonly AppDbContext _context;

    public RecipeService(AppDbContext context)
    {
        _context = context;
    }


    public async Task<List<Recipe>> GetAllAsync()
    {
        return await _context.Recipes

            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)

            .Include(r => r.Reviews)

            .OrderByDescending(r => r.CreatedAt)

            .ToListAsync();
    }



    public async Task<List<Recipe>> GetAllRecipesForAdminAsync()
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



    public async Task AddAsync(
        Recipe recipe,
        List<RecipeIngredient> ingredients)
    {
        recipe.CreatedAt = DateTime.Now;

        recipe.RecipeIngredients = ingredients;

        _context.Recipes.Add(recipe);

        await _context.SaveChangesAsync();
    }


    public async Task UpdateAsync(
        Recipe recipe,
        List<RecipeIngredient> ingredients)
    {
        var existingRecipe =
            await _context.Recipes

            .Include(r => r.RecipeIngredients)

            .FirstOrDefaultAsync(r => r.Id == recipe.Id);

        if(existingRecipe == null)
        {
            return;
        }

        existingRecipe.Title =
            recipe.Title;

        existingRecipe.Description =
            recipe.Description;

        existingRecipe.Category =
            recipe.Category;

        existingRecipe.CuisineType =
            recipe.CuisineType;

        existingRecipe.ImageUrl =
            recipe.ImageUrl;

        existingRecipe.Servings =
            recipe.Servings;

        existingRecipe.AuthorName =
            recipe.AuthorName;

        /* REMOVE OLD INGREDIENTS */

        _context.RecipeIngredients
            .RemoveRange(existingRecipe.RecipeIngredients);

        /* ADD NEW INGREDIENTS */

        existingRecipe.RecipeIngredients = ingredients;

        await _context.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id)
    {
        var recipe = await _context.Recipes

            .Include(r => r.RecipeIngredients)

            .Include(r => r.Reviews)

            .FirstOrDefaultAsync(r => r.Id == id);

        if(recipe != null)
        {
       

            _context.RecipeIngredients
                .RemoveRange(recipe.RecipeIngredients);


            _context.Reviews
                .RemoveRange(recipe.Reviews);

   
            _context.Recipes.Remove(recipe);

            await _context.SaveChangesAsync();
        }
    }


    public async Task DeleteRecipeAsync(int id)
    {
        var recipe = await _context.Recipes

            .Include(r => r.RecipeIngredients)

            .Include(r => r.Reviews)

            .FirstOrDefaultAsync(r => r.Id == id);

        if(recipe != null)
        {
            _context.RecipeIngredients
                .RemoveRange(recipe.RecipeIngredients);

            _context.Reviews
                .RemoveRange(recipe.Reviews);

            _context.Recipes.Remove(recipe);

            await _context.SaveChangesAsync();
        }
    }


    public async Task<List<Recipe>> SearchAsync(string keyword)
    {
        if(string.IsNullOrWhiteSpace(keyword))
        {
            return await GetAllAsync();
        }

        keyword = keyword.ToLower();

        return await _context.Recipes

            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)

            .Where(r =>

                r.Title.ToLower().Contains(keyword)

                ||

                r.Description.ToLower().Contains(keyword)

                ||

                r.Category.ToLower().Contains(keyword)

                ||

                r.CuisineType.ToLower().Contains(keyword)

                ||

                r.AuthorName.ToLower().Contains(keyword)
            )

            .OrderByDescending(r => r.CreatedAt)

            .ToListAsync();
    }

    //TOTAL CALORIES

    public double GetTotalCalories(Recipe recipe)
    {
        return recipe.RecipeIngredients.Sum(ri =>

            ri.Quantity *

            (ri.Ingredient?.CaloriesPerUnit ?? 0)
        );
    }

    //CALORIES PER PERSON

    public double GetCaloriesPerPerson(Recipe recipe)
    {
        if(recipe.Servings <= 0)
        {
            return 0;
        }

        return GetTotalCalories(recipe)
            / recipe.Servings;
    }

    //TOTAL RECIPES COUNT

    public async Task<int> CountAsync()
    {
        return await _context.Recipes.CountAsync();
    }

    /* =========================================
       CATEGORY COUNT
    ========================================= */

    public async Task<int> BreakfastCountAsync()
    {
        return await _context.Recipes
            .CountAsync(r => r.Category == "Petit-déjeuner");
    }

    public async Task<int> LunchCountAsync()
    {
        return await _context.Recipes
            .CountAsync(r => r.Category == "Déjeuner");
    }

    public async Task<int> DinnerCountAsync()
    {
        return await _context.Recipes
            .CountAsync(r => r.Category == "Dîner");
    }

    public async Task<int> DessertCountAsync()
    {
        return await _context.Recipes
            .CountAsync(r => r.Category == "Dessert");
    }
}