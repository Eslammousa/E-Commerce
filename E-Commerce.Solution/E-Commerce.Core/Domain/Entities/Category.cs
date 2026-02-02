namespace E_Commerce.Core.Domain.Entities
{
    public class Category : BaseClass
    {
       
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
