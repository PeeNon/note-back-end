namespace NotesApi.Dto
{
    public class PagedResponseDto<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Total { get; set; }

        public PagedResponseDto(IEnumerable<T> data, int total)
        {
            Data = data;
            Total = total;
        }
    }
}