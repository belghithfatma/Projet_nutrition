using app_nutri.Data;
using app_nutri.Models;
using Microsoft.EntityFrameworkCore;

namespace app_nutri.Services;

public class IngredientService
{
    private readonly AppDbContext _context;

    public IngredientService(AppDbContext context)
    {
        _context = context;
    }

   
    public async Task<List<Ingredient>> GetAllAsync()
    {
        return await _context.Ingredients
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    public async Task AddAsync(Ingredient ingredient)
    {
        _context.Ingredients.Add(ingredient);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Ingredient ingredient)
    {
        var existingIngredient =
            await _context.Ingredients
            .FirstOrDefaultAsync(i => i.Id == ingredient.Id);

        if (existingIngredient != null)
        {
            existingIngredient.Name =
                ingredient.Name;

            existingIngredient.Unit =
                ingredient.Unit;

            existingIngredient.CaloriesPerUnit =
                ingredient.CaloriesPerUnit;

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var ingredient =
            await _context.Ingredients.FindAsync(id);

        if (ingredient != null)
        {
            _context.Ingredients.Remove(ingredient);

            await _context.SaveChangesAsync();
        }
    }
}