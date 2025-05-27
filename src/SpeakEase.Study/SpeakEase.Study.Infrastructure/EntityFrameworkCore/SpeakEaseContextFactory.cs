using Microsoft.EntityFrameworkCore;

namespace SpeakEase.Study.Infrastructure.EntityFrameworkCore;

public class SpeakEaseContextFactory(IDbContextFactory<SpeakEaseStudyContext> pooleFactory) : IDbContextFactory<SpeakEaseStudyContext>
{
    public SpeakEaseStudyContext CreateDbContext()
    {
        return pooleFactory.CreateDbContext();
    }
}