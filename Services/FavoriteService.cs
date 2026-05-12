using app_nutri.Data;
using app_nutri.Models;

using Microsoft.EntityFrameworkCore;

namespace app_nutri.Services;

public class FavoriteService
{
    private readonly AppDbContext _context;

    public FavoriteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task ToggleFavoriteAsync(int recipeId)
    {
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(f => f.RecipeId == recipeId);

        if (favorite != null)
        {
            _context.Favorites.Remove(favorite);
        }
        else
        {
            _context.Favorites.Add(new Favorite
            {
                RecipeId = recipeId
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsFavoriteAsync(int recipeId)
    {
        return await _context.Favorites
            .AnyAsync(f => f.RecipeId == recipeId);
    }

    public async Task<List<int>> GetFavoriteIdsAsync()
    {
        return await _context.Favorites
            .Select(f => f.RecipeId)
            .ToListAsync();
    }
}