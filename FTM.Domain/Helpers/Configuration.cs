using Microsoft.Extensions.Configuration;

namespace FTM.Domain.Helpers;

public class Configuration
{
    public static IConfigurationRoot Root { get; private set; }

    public static void Init(IConfigurationRoot configurationRoot)
    {
        Root = configurationRoot;
    }
}