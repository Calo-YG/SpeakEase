namespace SpeakEase.Contract.Users.Dto
{
    /// <summary>
    /// 用户设置创建请求
    /// </summary>
    public sealed class UserSettingInput
    {
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
    }
}
