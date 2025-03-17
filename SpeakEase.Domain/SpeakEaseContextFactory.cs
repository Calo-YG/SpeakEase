using Microsoft.EntityFrameworkCore;

namespace SpeakEase.Domain;

public class SpeakEaseContextFactory(IDbContextFactory<SpeakEaseContext> pooleFactory):IDbContextFactory<SpeakEaseContext>
{
      public SpeakEaseContext CreateDbContext()=> pooleFactory.CreateDbContext();
}