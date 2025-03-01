namespace NotesApi.Dto
{
    public class UserRegisterDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string PasswordConfirm { get; set; }
    }
}