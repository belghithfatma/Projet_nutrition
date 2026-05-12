using app_nutri.Data;
using Microsoft.EntityFrameworkCore;

namespace app_nutri.Services;

public class StatisticsService
{
    private readonly AppDbContext _context;

    public StatisticsService(AppDbContext context) => _context = context;

    public async Task<int> TotalRecipesAsync() => await _context.Recipes.CountAsync();
    public async Task<int> TotalIngredientsAsync() => await _context.Ingredients.CountAsync();
    public async Task<int> TotalReviewsAsync() => await _context.Reviews.CountAsync();

    public async Task<Dictionary<string, int>> RecipesByCategoryAsync() =>
        await _context.Recipes.GroupBy(r => r.Category).ToDictionaryAsync(g => g.Key, g => g.Count());

    public async Task<Dictionary<string, int>> RecipesByCuisineAsync() =>
        await _context.Recipes.GroupBy(r => r.CuisineType).ToDictionaryAsync(g => g.Key, g => g.Count());
}
