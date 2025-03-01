namespace NotesApi.Models
{
    public class User : BaseModel
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public  required string Password { get; set; }

        public ICollection<Note> Notes { get; set; } = [];
    }
}