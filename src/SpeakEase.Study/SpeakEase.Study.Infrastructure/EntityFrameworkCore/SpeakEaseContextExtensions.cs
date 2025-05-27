using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SpeakEase.Study.Infrastructure.EntityFrameworkCore;

public static class SpeakEaseContextExtensions
{
    public static IServiceCollection RegisterEntityFrameworkCoreContext(this IServiceCollection sevices, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        sevices.AddDbContext<IDbContext, SpeakEaseStudyContext>((builder) =>
        {
            builder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), options =>
            {

            });
        },contextLifetime:ServiceLifetime.Scoped,optionsLifetime:ServiceLifetime.Scoped);

        return sevices;
    }
}