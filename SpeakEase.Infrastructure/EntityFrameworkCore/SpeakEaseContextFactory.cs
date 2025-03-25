using Microsoft.EntityFrameworkCore;

namespace SpeakEase.Infrastructure.EntityFrameworkCore;

public class SpeakEaseContextFactory(IDbContextFactory<SpeakEaseContext> pooleFactory) : IDbContextFactory<SpeakEaseContext>
{
    public SpeakEaseContext CreateDbContext() => pooleFactory.CreateDbContext();
}