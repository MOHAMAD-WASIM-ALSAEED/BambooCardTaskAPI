namespace BambooCardAPI.Services
{
    public interface ICacheService
    {
        T Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan expirationTime);
        bool Remove(string key);
    }
}
