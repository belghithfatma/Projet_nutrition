using app_nutri.Data;
using Microsoft.EntityFrameworkCore;

namespace app_nutri.Services;

public class DashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetUsersCountAsync()
    {
        return await _context.Users.CountAsync();
    }

    public async Task<int> GetIngredientsCountAsync()
    {
        return await _context.Ingredients.CountAsync();
    }

    public async Task<int> GetRecipesCountAsync()
    {
        return await _context.Recipes.CountAsync();
    }

    public async Task<double> GetCaloriesAsync()
    {
        return await _context.Ingredients
            .SumAsync(i => i.CaloriesPerUnit);
    }

    public async Task<List<string>> GetRecentActivitiesAsync()
    {
        var activities = new List<string>();

        var users = await _context.Users
            .OrderByDescending(u => u.Id)
            .Take(3)
            .ToListAsync();

        foreach(var user in users)
        {
            activities.Add($"{user.FullName} joined platform");
        }

        return activities;
    }
}