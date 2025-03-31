using Microsoft.EntityFrameworkCore;
using SpeakEase.Domain.Shared;

namespace SpeakEase.Domain
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder BuilderCration<T>(this ModelBuilder builder) where T : class,ICreation
        {
            builder.Entity<T>(op =>
            {
                op.Property(p => p.CreateByName).HasMaxLength(50);
                op.Property(p=>p.CreateById).HasMaxLength(50);
            });

            return builder;
        }

        public static ModelBuilder BuilderModify<T>(this ModelBuilder builder) where T : class, IModify
        {
            builder.Entity<T>(op =>
            {
                op.Property(p => p.ModifyByName).HasMaxLength(50);
                op.Property(p => p.ModifyById).HasMaxLength(50);
            });

            return builder;
        }
    }
}
