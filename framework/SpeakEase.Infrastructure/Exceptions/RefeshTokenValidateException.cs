namespace SpeakEase.Infrastructure.Exceptions
{
    public sealed class RefeshTokenValidateException:Exception
    {
        public RefeshTokenValidateException()
        {
        }

        public RefeshTokenValidateException(string message) : base(message)
        {
        }
    }
}
