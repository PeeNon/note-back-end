using Swashbuckle.AspNetCore.Annotations;

namespace NotesApi.Dto
{
    [SwaggerSchema(Required = new[] { "Title", "Content" })]
    public class NoteDto
    {
        [SwaggerSchema("The title of the note")]
        public required string Title { get; set; }

        [SwaggerSchema("The content of the note")]
        public string? Content { get; set; }
    }
}
