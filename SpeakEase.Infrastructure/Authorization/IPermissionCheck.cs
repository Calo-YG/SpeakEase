namespace SpeakEase.Infrastructure.Authorization;

public interface IPermissionCheck
{
    Task<bool> IsGranted(long userId, string[] authorizationNames);
}