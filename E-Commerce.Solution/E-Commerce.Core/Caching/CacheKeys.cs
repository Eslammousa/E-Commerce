namespace E_Commerce.Core.Caching
{
    public static class CacheKeys
    {
        public const string ProductsAll = "products:all";
        public const string ProductPrefix = "products:";
        public const string CategoriesAll = "categories:all";
        public const string CategoryPrefix = "categories:";

        public static string ProductById(Guid id) => $"products:{id}";
        public static string CategoryById(Guid id) => $"categories:{id}";
        public static string ProductsByCategory(Guid categoryId) => $"products:cat:{categoryId}";
    }
}
