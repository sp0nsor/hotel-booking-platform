namespace HotelService.Core.Common
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = [];
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
