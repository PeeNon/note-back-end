using Swashbuckle.AspNetCore.Annotations;

namespace NotesApi.Dto
{
     [SwaggerSchema(Required = new[] { "Username", "Password" })]
    public class UserLoginDto
    {
        [SwaggerSchema("The username of the user")]
        public required string Username { get; set; }

        [SwaggerSchema("The password of the user")]
        public required string Password { get; set; }
    }
}