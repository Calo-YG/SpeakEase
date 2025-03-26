using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpeakEase.Domain.Users.Enum;

namespace SpeakEase.Infrastructure.EntityFrameworkCore;

public static class SpeakEaseContextExtensions
{
    public static IServiceCollection RegisterEntityFrameworkCoreContext(this IServiceCollection sevices, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        sevices.AddDbContext<IDbContext, SpeakEaseContext>((builder) =>
        {
            builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), options =>
            {
                options.MapEnum<SourceEnum>();
            });
        },contextLifetime:ServiceLifetime.Scoped,optionsLifetime:ServiceLifetime.Scoped);
        //sevices.AddScoped<SpeakEaseDesignFactory>();

        return sevices;
    }
}