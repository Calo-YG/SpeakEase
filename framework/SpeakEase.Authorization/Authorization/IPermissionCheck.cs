namespace SpeakEase.Authorization.Authorization;

public interface IPermissionCheck
{
    Task<bool> IsGranted(string userId, string authorizationNames);
}