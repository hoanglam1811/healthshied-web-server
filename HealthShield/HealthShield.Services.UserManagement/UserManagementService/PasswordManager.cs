namespace UserManagementService;

public class PasswordManager
{
    public static string GeneratePassword()
    {
        return BCrypt.Net.BCrypt.GenerateSalt(10);
    }

    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
