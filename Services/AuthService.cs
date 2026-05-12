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

    public async Task<User?> LoginAsync(string email, string password)
    {
        var user = await _userService.GetByEmailAsync(email);

        if (user == null)
            return null;

        bool isPasswordValid = PasswordHasher.VerifyPassword(
            password,
            user.PasswordHash
        );

        if (!isPasswordValid)
            return null;

        CurrentUser = user;
        _authStateProvider.MarkUserAsAuthenticated(user);

        return user;
    }

    public async Task<User> RegisterAsync(
        string fullName,
        string email,
        string password)
    {
        var user = await _userService.RegisterAsync(
            fullName,
            email,
            password,
            UserRole.User
        );

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