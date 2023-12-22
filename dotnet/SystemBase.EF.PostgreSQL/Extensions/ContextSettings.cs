using Microsoft.EntityFrameworkCore;

namespace SystemBase;

public static class ContextSettingsExtensions
{
    public static void UseNpgsql(this ContextSettings settings, string connectionString)
    {
        settings.ConfigureOptionsBuilder = options => options.UseNpgsql(connectionString);
    }
}