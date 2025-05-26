using Microsoft.EntityFrameworkCore;

namespace SpeakEase.Study.Infrastructure.EntityFrameworkCore;

public class SpeakEaseContextFactory(IDbContextFactory<SpeakEaseContext> pooleFactory) : IDbContextFactory<SpeakEaseContext>
{
    public SpeakEaseContext CreateDbContext()
    {
        return pooleFactory.CreateDbContext();
    }
}