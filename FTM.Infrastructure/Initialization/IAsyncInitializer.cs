namespace FTM.Infrastructure.Initialization;

public interface IAsyncInitializer
{
    Task Initialize();
}