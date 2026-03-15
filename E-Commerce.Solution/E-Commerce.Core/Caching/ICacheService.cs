namespace E_Commerce.Core.Caching
{
    public interface ICacheService
    {
        T? Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan? expiration = null);
        void Remove(string key);
        void RemoveByPrefix(string prefix);
    }
}
