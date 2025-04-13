using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpeakEase.Domain.Friend;
using SpeakEase.Domain.Friend.Enum;

namespace SpeakEase.Infrastructure.EntityFrameworkCore.ModelBuilders
{
    public static class UserFriendModelBuilder
    {
        public static ModelBuilder ConfigureModelUserFriend(this ModelBuilder builder)
        {
            builder.Entity<UserFriendEntity>(op =>
            {
                op.HasKey(x => x.Id);
                op.Property(x => x.UserId).IsRequired();
                op.Property(x=>x.Remark).IsRequired().HasMaxLength(20);
                op.Property(x=>x.FriendUserId).IsRequired();
                op.Property(x=>x.FriendUserRemark).IsRequired().HasMaxLength(20);
                op.Property(p => p.Status).HasConversion(new ValueConverter<FriendStatus, int>(
                         v => ((int)v),
                         v => (FriendStatus)v)).HasDefaultValue(FriendStatus.Pending);
                op.Property(p => p.CreatedAt).IsRequired();
            });

            return builder;
        }
    }
}
