using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpeakEase.Domain;
using SpeakEase.Domain.Users;
using SpeakEase.Domain.Users.Enum;

namespace SpeakEase.Infrastructure.EntityFrameworkCore.ModelBuilders
{
    public static class UserModelBuilder
    {
        public static ModelBuilder ConfigureModelUser(this ModelBuilder builder)
        {

            builder.Entity<UserEntity>(op =>
            {
                op.HasKey(p => p.Id);
                op.Property(p => p.UserPassword).IsRequired().HasMaxLength(128);
                op.Property(p => p.UserAccount).IsRequired().HasMaxLength(50);
                op.Property(p => p.Source).HasConversion(new ValueConverter<SourceEnum, int>(
                v => ((int)v),
                v => (SourceEnum)v));
            });

            builder.Entity<RefreshTokenEntity>(op =>
            {
                op.HasKey(p => p.Id);
                op.Property(p => p.Token).IsRequired();
                op.Property(p => p.UserId).IsRequired();
                op.Property(p => p.IsUsed).IsRequired().HasDefaultValue(false);
                op.Property(p => p.Expires).IsRequired();
            });

            builder.Entity<UserSettingEntity>(op =>
            {
                op.ToTable("user_setting");
                op.HasKey(p => p.Id);
                op.Property(p => p.IsProfilePublic).IsRequired().HasDefaultValue(false);
                op.Property(p => p.ShowLearningProgress).IsRequired().HasDefaultValue(false);
                op.Property(p => p.AllowMessages).IsRequired().HasDefaultValue(false);
                op.Property(p => p.ReceiveNotifications).IsRequired().HasDefaultValue(false);
                op.Property(p => p.ReceiveEmailUpdates).IsRequired().HasDefaultValue(false);
                op.Property(p => p.ReceivePushNotifications).IsRequired().HasDefaultValue(false);
                op.Property(p => p.AccountActive).IsRequired().HasDefaultValue(false);
            });


            builder.Entity<UserWordEntity>(op =>
            {
                op.ToTable("user_word");
                op.HasKey(p => p.Id);
            })
            .BuilderCration<UserWordEntity>();

            return builder;
        }
    }
}
