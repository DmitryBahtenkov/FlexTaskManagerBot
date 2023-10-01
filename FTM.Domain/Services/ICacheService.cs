namespace FTM.Domain.Services;

public interface ICacheService
{
    public Task<TValue?> GetOrCreate<TValue>(string key, Func<Task<TValue>> factory, TimeSpan? duration = null);
    public Task<TValue?> CreateEntry<TValue>(string key, Func<Task<TValue>> factory, TimeSpan? duration = null);
    public Task<TValue?> GetEntry<TValue>(string key);
}