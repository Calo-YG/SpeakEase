using SpeakEase.Study.Domain.Shared;

namespace SpeakEase.Study.Domain.Users
{
    /// <summary>
    /// 用户设置
    /// </summary>
    public class UserSettingEntity:Entity<long>
    {
        /// <summary>外键：用户 ID，不可为空</summary>
        public long UserId { get; private set; }

        /// <summary>个人简介，可为 null</summary>
        public string Bio { get; private set; }

        /// <summary>性别，可为 null</summary>
        public string Gender { get; private set; }

        /// <summary>生日，可为 null</summary>
        public DateTime? Birthday { get; private set; }

        /// <summary>背景图片 URL，可为 null</summary>
        public string BackgroundImage { get; private set; }

        /// <summary>个人资料是否公开</summary>
        public bool IsProfilePublic { get; private set; }

        /// <summary>是否允许接收私信</summary>
        public bool AllowMessages { get; private set; }

        /// <summary>是否接收邮件更新</summary>
        public bool ReceiveEmailUpdates { get; private set; }

        /// <summary>是否接收通知</summary>
        public bool ReceiveNotifications { get; private set; }

        /// <summary>是否接收推送通知</summary>
        public bool ReceivePushNotifications { get; private set; }

        /// <summary>是否展示学习进度</summary>
        public bool ShowLearningProgress { get; private set; }

        /// <summary>创建时间，数据库自动维护</summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>更新时间，数据库自动维护</summary>
        public DateTime ModifyAt { get; private set; }

        // EF Core 物化实体时需调用无参构造
        private UserSettingEntity() { }

        /// <summary>
        /// 公共构造函数：初始化所有需外部赋值的属性
        /// </summary>
        public UserSettingEntity(
            long userId,
            string bio,
            string gender,
            DateTime? birthday,
            string backgroundImage,
            bool isProfilePublic,
            bool allowMessages,
            bool receiveEmailUpdates,
            bool receiveNotifications,
            bool receivePushNotifications,
            bool showLearningProgress)
        {
            UserId = userId;
            Bio = bio;
            Gender = gender;
            Birthday = birthday;
            BackgroundImage = backgroundImage;
            IsProfilePublic = isProfilePublic;
            AllowMessages = allowMessages;
            ReceiveEmailUpdates = receiveEmailUpdates;
            ReceiveNotifications = receiveNotifications;
            ReceivePushNotifications = receivePushNotifications;
            ShowLearningProgress = showLearningProgress;
            CreatedAt = DateTime.Now;
        }

        public void Modify(string bio,
            string gender,
            DateTime? birthday,
            string backgroundImage,
            bool isProfilePublic,
            bool allowMessages,
            bool receiveEmailUpdates,
            bool receiveNotifications,
            bool receivePushNotifications,
            bool showLearningProgress)
        {
            Bio = bio;
            Gender = gender;
            Birthday = birthday;
            BackgroundImage = backgroundImage;
            IsProfilePublic = isProfilePublic;
            AllowMessages = allowMessages;
            ReceiveEmailUpdates = receiveEmailUpdates;
            ReceiveNotifications = receiveNotifications;
            ReceivePushNotifications = receivePushNotifications;
            ShowLearningProgress = showLearningProgress;
            ModifyAt = DateTime.Now;
        }
    }
}
