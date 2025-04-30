using menus.Models;
using menus.Services;

public class SimpleAuthService : IAuthService
{
    private static List<User> _users = new List<User>
    {
        new User { Username = "admin", Password = "1234" },
        new User { Username = "test", Password = "test" }
    };

    public bool Authenticate(string username, string password)
    {
        return _users.Any(u => u.Username == username && u.Password == password);
    }
}