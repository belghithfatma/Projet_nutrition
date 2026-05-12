using app_nutri.Data;
using app_nutri.Models;
using Microsoft.EntityFrameworkCore;

namespace app_nutri.Services;

public class ReviewService
{
    private readonly AppDbContext _context;

    public ReviewService(AppDbContext context) => _context = context;

    public async Task AddAsync(Review review)
    {
        review.CreatedAt = DateTime.Now;
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Review>> GetByRecipeAsync(int recipeId) =>
        await _context.Reviews.Where(r => r.RecipeId == recipeId).OrderByDescending(r => r.CreatedAt).ToListAsync();
}
