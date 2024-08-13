namespace Athenticate.Service;
using Athenticate.Interface;
using Athenticate.Database;
using System.Threading.Tasks;
using Athenticate.Dto;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Athenticate.Model;

public class UserService : IUserService
{
    private readonly DataContext _context;
    public UserService(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> AddUser(RegisterDto register)
    {
        var sql = "INSERT INTO Users(Name,Email,PasswordHash,CreatedDate) VALUES(@p0,@p1,@p2,@p3)";
        int affetedRows = await _context.Database.ExecuteSqlRawAsync(sql, register.Name, register.Email, HashPassword(register.Password), DateTime.Now);
        return affetedRows > 0;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var sql = "SELECT * FROM Users WHERE Email = @p0 LIMIT 1";
        return await _context.Users.FromSqlRaw(sql, email).FirstOrDefaultAsync();
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}