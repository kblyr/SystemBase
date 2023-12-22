using Microsoft.EntityFrameworkCore;

namespace SystemBase;

public static class ContextSettingsExtensions
{
    public static void UseSqlServer(this ContextSettings settings, string connectionString)
    {
        settings.ConfigureOptionsBuilder = options => options.UseSqlServer(connectionString);
    }
}