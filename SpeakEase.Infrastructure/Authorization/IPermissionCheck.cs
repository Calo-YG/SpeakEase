namespace SpeakEase.Infrastructure.Authorization;

public interface IPermissionCheck
{
      Task<bool> IsGranted(string userId, string[] authorizationNames);
}