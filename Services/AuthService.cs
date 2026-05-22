using app_nutri.Models;

namespace app_nutri.Services;

public class AuthService
{
    private readonly UserService _userService;

    private readonly CustomAuthStateProvider _authStateProvider;

    public AuthService(
        UserService userService,
        CustomAuthStateProvider authStateProvider)
    {
        _userService = userService;

        _authStateProvider = authStateProvider;
    }


    public User? CurrentUser { get; private set; }

    public async Task<User?> LoginAsync(
        string email,
        string password)
    {
        var user =
            await _userService.GetByEmailAsync(email);

        if(user == null)
        {
            return null;
        }

        bool isPasswordValid =
            PasswordHasher.VerifyPassword(
                password,
                user.PasswordHash
            );

        if(!isPasswordValid)
        {
            return null;
        }

        CurrentUser = user;

        _authStateProvider.MarkUserAsAuthenticated(user);

        return user;
    }


    public async Task<User?> RegisterAsync(
        string fullName,
        string email,
        string password)
    {
    

        var existingUser =
            await _userService.GetByEmailAsync(email);

        if(existingUser != null)
        {
            return null;
        }



        string hashedPassword =
            PasswordHasher.HashPassword(password);

 

        var user = new User
        {
            FullName = fullName,

            Email = email,

            PasswordHash = hashedPassword,

            Role = UserRole.User
        };


        await _userService.AddAsync(user);

        CurrentUser = user;

        _authStateProvider.MarkUserAsAuthenticated(user);

        return user;
    }


    public void Logout()
    {
        CurrentUser = null;

        _authStateProvider.MarkUserAsLoggedOut();
    }
}