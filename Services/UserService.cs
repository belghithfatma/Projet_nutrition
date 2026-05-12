using Microsoft.EntityFrameworkCore;
using app_nutri.Data;
using app_nutri.Models;

namespace app_nutri.Services;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .OrderBy(u => u.FullName)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User> RegisterAsync(
        string fullName,
        string email,
        string password,
        UserRole role = UserRole.User)
    {
        bool exists = await _context.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());

        if (exists)
            throw new Exception("Cet email est déjà utilisé.");

        var user = new User
        {
            FullName = fullName,
            Email = email,
            PasswordHash = PasswordHasher.HashPassword(password),
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);

        if (existingUser == null)
            throw new Exception("Utilisateur introuvable.");

        existingUser.FullName = user.FullName;
        existingUser.Email = user.Email;
        existingUser.Role = user.Role;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            throw new Exception("Utilisateur introuvable.");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}