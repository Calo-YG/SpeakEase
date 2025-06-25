namespace SpeakEase.Infrastructure.Exceptions
{
    public sealed class RefreshTokenValidateException:Exception
    {
        public RefreshTokenValidateException()
        {
        }

        public RefreshTokenValidateException(string message) : base(message)
        {
        }
    }
}
