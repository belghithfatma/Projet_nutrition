using app_nutri.Data;
using app_nutri.Models;
using Microsoft.EntityFrameworkCore;

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
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

 

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);

        await _context.SaveChangesAsync();
    }



    public async Task<User> RegisterAsync(
        string fullName,
        string email,
        string passwordHash,
        UserRole role)
    {
        var user = new User
        {
            FullName = fullName,

            Email = email,

            PasswordHash = passwordHash,

            Role = role
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }


    public async Task UpdateAsync(User updatedUser)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == updatedUser.Id);

        if(existingUser != null)
        {
            existingUser.FullName =
                updatedUser.FullName;

            existingUser.Email =
                updatedUser.Email;

            existingUser.Role =
                updatedUser.Role;

            await _context.SaveChangesAsync();
        }
    }



    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if(user != null)
        {
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }
    }



    public async Task<List<User>> SearchAsync(string keyword)
    {
        if(string.IsNullOrWhiteSpace(keyword))
        {
            return await GetAllAsync();
        }

        keyword = keyword.ToLower();

        return await _context.Users
            .Where(u =>

                u.FullName.ToLower().Contains(keyword)

                ||

                u.Email.ToLower().Contains(keyword)

                ||

                u.Role.ToString().ToLower().Contains(keyword)
            )
            .OrderBy(u => u.FullName)
            .ToListAsync();
    }



    public async Task<int> CountAsync()
    {
        return await _context.Users.CountAsync();
    }

    public async Task<int> AdminCountAsync()
    {
        return await _context.Users
            .CountAsync(u => u.Role == UserRole.Admin);
    }


    public async Task<int> NormalUsersCountAsync()
    {
        return await _context.Users
            .CountAsync(u => u.Role == UserRole.User);
    }
}