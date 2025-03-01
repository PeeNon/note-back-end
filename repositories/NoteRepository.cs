using Dapper;
using NotesApi.Models;
using NotesApi.Dto;
using System.Data;

namespace NotesApi.Repositories
{
    public class NoteRepository
    {
        private readonly AppDbContext _context;

        public NoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync(NoteQueryParameters queryParameters, string userId)
        {
            using (IDbConnection db = _context.CreateConnection())
            {
                var sql = "SELECT * FROM Notes WHERE UserId = @UserId";
                if (!string.IsNullOrEmpty(queryParameters.SearchQuery))
                {
                    sql += " AND Title LIKE @SearchQuery";
                }
                if(!string.IsNullOrEmpty(queryParameters.Dfrom) && !string.IsNullOrEmpty(queryParameters.Dto))
                {
                    sql += " AND CreatedAt BETWEEN @Dfrom AND @Dto";
                }
                if (!string.IsNullOrEmpty(queryParameters.Sort) && !string.IsNullOrEmpty(queryParameters.Order))
                {
                    sql += $" ORDER BY {queryParameters.Sort} {queryParameters.Order}";
                }
                sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                return await db.QueryAsync<Note>(sql, new { 
                    SearchQuery = $"%{queryParameters.SearchQuery}%",
                    PageSize = queryParameters.PageSize, 
                    Offset = (queryParameters.PageNumber - 1) * queryParameters.PageSize,
                    UserId = userId ,
                    Dfrom = $"{queryParameters.Dfrom} 00:00:00",
                    Dto = $"{queryParameters.Dto} 23:59:59",
                });
            }
        }

        public async Task<int> CountAllNotesAsync(NoteQueryParameters queryParameters, string userId)
        {
            using (IDbConnection db = _context.CreateConnection())
            {
                var sql = "SELECT COUNT(*) FROM Notes WHERE UserId = @UserId";
                if (!string.IsNullOrEmpty(queryParameters.SearchQuery))
                {
                    sql += " AND Title LIKE @SearchQuery";
                }
                if(!string.IsNullOrEmpty(queryParameters.Dfrom) && !string.IsNullOrEmpty(queryParameters.Dto))
                {
                    sql += " AND CreatedAt BETWEEN @Dfrom AND @Dto";
                }
                return await db.ExecuteScalarAsync<int>(sql, new { 
                    SearchQuery = $"%{queryParameters.SearchQuery}%", 
                    UserId = userId, 
                    Dfrom = $"{queryParameters.Dfrom} 00:00:00",
                    Dto = $"{queryParameters.Dto} 23:59:59",
                });
            }
        }

        public async Task<Note> GetNoteByIdAsync(int id, string userId)
        {
            using (IDbConnection db = _context.CreateConnection())
            {
                var sql = "SELECT * FROM Notes WHERE Id = @Id AND UserId = @UserId";
                return await db.QueryFirstOrDefaultAsync<Note>(sql, new { Id = id, UserId = userId });
            }
        }

        public async Task<Note> CreateNoteAsync(NoteDto createNoteDto, string userId)
        {
            using (IDbConnection db = _context.CreateConnection())
            {
                var note = new Note
                {
                    Title = createNoteDto.Title,
                    Content = createNoteDto.Content,
                    UserId = int.Parse(userId),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var sql = "INSERT INTO Notes (Title, Content, UserId, CreatedAt, UpdatedAt) VALUES (@Title, @Content, @UserId, @CreatedAt, @UpdatedAt)";
                await db.ExecuteAsync(sql, note);
                return note;
            }
        }

        public async Task<int> UpdateNoteAsync(int id, NoteDto updateNoteDto, string userId)
        {
            using (IDbConnection db = _context.CreateConnection())
            {   var note = new Note
                {
                    Id = id,
                    Title = updateNoteDto.Title,
                    Content = updateNoteDto.Content,
                    UpdatedAt = DateTime.UtcNow,
                    UserId = int.Parse(userId)
                };
                var sql = "UPDATE Notes SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt WHERE Id = @Id AND UserId = @UserId";
                return await db.ExecuteAsync(sql, note);
            }
        }

        public async Task<int> DeleteNoteAsync(int id, string userId)
        {
            using (IDbConnection db = _context.CreateConnection())
            {
                var sql = "DELETE FROM Notes WHERE Id = @Id AND UserId = @UserId";
                return await db.ExecuteAsync(sql, new { Id = id, UserId = userId });
            }
        }

        public async Task<int> DeleteManyNoteAsync(int[] ids, string userId)
        {
            using (IDbConnection db = _context.CreateConnection())
            {
                var sql = "DELETE FROM Notes WHERE Id IN @Ids AND UserId = @UserId";
                return await db.ExecuteAsync(sql, new { Ids = ids, UserId = userId});
            }
        }
    }
}