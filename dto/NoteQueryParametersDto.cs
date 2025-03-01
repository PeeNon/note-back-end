using NotesApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace NotesApi.Dto
{
    public class NoteQueryParameters
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int PageNumber { get; set; } = 1;

        [Range(1, int.MaxValue, ErrorMessage = "Page size must be greater than 0.")]
        public int PageSize { get; set; } = 10;

        public string? SearchQuery { get; set; }

        [SortValidation]
        public string? Sort { get; set; }

        [OrderValidation]
        public string? Order { get; set; }

        public string? Dfrom { get; set; }
        public string? Dto { get; set; }
    }
}