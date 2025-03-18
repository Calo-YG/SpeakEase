using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SpeakEase.Domain;

public static class SpeakEaseContextExtensions
{
    public static IServiceCollection RegisterEntityFrameworkCoreContext(this IServiceCollection sevices, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        sevices.AddDbContext<SpeakEaseContext>(option => option.UseNpgsql(connectionString));
        sevices.AddScoped<SpeakEaseContextFactory>();
        sevices.AddScoped(sp => sp.GetRequiredService<SpeakEaseContextFactory>().CreateDbContext());

        return sevices;
    }
}