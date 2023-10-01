using FTM.Domain.Helpers;
using StackExchange.Redis;

namespace FTM.Infrastructure.Redis;

public class RedisConnection
{
    private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

    static RedisConnection()
    {
        var connection = Environment.GetEnvironmentVariable("REDIS_CONNECTION") ?? Configuration.Root["RedisConnection"];
        var password = Environment.GetEnvironmentVariable("REDIS_PASSWORD");
        var user = Environment.GetEnvironmentVariable("REDIS_USER");
        var configurationOptions = new ConfigurationOptions
        {
            EndPoints = { connection },
            Password = password,
            User = user
        };

        LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
    }

    public static ConnectionMultiplexer Connection => LazyConnection.Value;

    public static IDatabase RedisCache => Connection.GetDatabase();
}