namespace E_Commerce.Core.Domain.Entities
{
    public class Category : BaseClass
    {
        public string Image { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
