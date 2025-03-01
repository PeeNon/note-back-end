using Dapper;
using NotesApi.Models;
using System.Data;

namespace NotesApi.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using (IDbConnection db = _context.CreateConnection())
            {
                var sql = "SELECT * FROM Users WHERE Username = @Username";
                return await db.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
            }
        }

        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return true;
            }
            return false;
        }

        public async Task<int> CreateUserAsync(User user)
        {
            using (IDbConnection db = _context.CreateConnection())
            {   
                user.CreatedAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;
                var sql = "INSERT INTO Users (Username, Password, CreatedAt, UpdatedAt) VALUES (@Username, @Password, @CreatedAt, @UpdatedAt)";
                return await db.ExecuteAsync(sql, user);
            }
        }
    }
}