using Microsoft.EntityFrameworkCore;
using SpeakEase.Domain.Users;

namespace SpeakEase.Infrastructure.EntityFrameworkCore.ModelBuilders
{
    public static class UserModelBuilder
    {
        public static ModelBuilder ConfigureModelUser(this ModelBuilder builder)
        {

            builder.Entity<UserEntity>(op =>
            {
                op.ToTable("User");
                op.HasKey(p => p.Id);
                op.Property(p => p.Password).IsRequired().HasMaxLength(128);
                op.Property(p => p.UserAccount).IsRequired().HasMaxLength(16);
                op.Property(p=>p.UserName).IsRequired().HasMaxLength(30);
                op.Property(p => p.Email).IsRequired().HasMaxLength(50);    
                op.Property(p=>p.Phone).IsRequired(false).HasMaxLength(20);
                op.Property(p => p.Avatar).IsRequired(false).HasMaxLength(255);
                op.HasIndex(p => p.UserAccount);
                op.HasIndex(p => p.Email);
            });

            builder.Entity<RefreshTokenEntity>(op =>
            {
                op.ToTable("RefreshToken");
                op.HasKey(p => p.Id);
                op.Property(p => p.Token).HasMaxLength(255).IsRequired();
                op.Property(p => p.UserId).IsRequired();
                op.Property(p => p.IsUsed).IsRequired().HasDefaultValue(false);
                op.Property(p => p.ExpireAt).IsRequired();
                op.HasIndex(p => new { p.UserId, p.Token });
            });

            builder.Entity<UserSettingEntity>(op =>
            {
                op.ToTable("UserSetting");
                op.HasKey(p => p.Id);
                op.HasIndex(p=>p.UserId);
                op.Property(p => p.IsProfilePublic).IsRequired().HasDefaultValue(false);
                op.Property(p => p.ShowLearningProgress).IsRequired().HasDefaultValue(false);
                op.Property(p => p.AllowMessages).IsRequired().HasDefaultValue(false);
                op.Property(p => p.ReceiveNotifications).IsRequired().HasDefaultValue(false);
                op.Property(p => p.ReceiveEmailUpdates).IsRequired().HasDefaultValue(false);
                op.Property(p => p.ReceivePushNotifications).IsRequired().HasDefaultValue(false);
            });

            return builder;
        }
    }
}
