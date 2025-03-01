namespace NotesApi.Models
{
    public class Note : BaseModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Content { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}