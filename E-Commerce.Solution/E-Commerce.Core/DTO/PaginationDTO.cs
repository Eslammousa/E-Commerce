namespace E_Commerce.Core.DTO
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;

        public int Size { get; set; } = 10;

        public string? SortBy { get; set; }

        public string sortDirection { get; set; } = "asc";
    }
} 
