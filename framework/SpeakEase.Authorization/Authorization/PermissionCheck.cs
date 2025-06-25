using SpeakEase.Infrastructure.SpeakEase.Core;

namespace SpeakEase.Authorization.Authorization;

public class PermissionCheck : IPermissionCheck
{
    public async Task<bool> IsGranted(string userId, string authorizationNames)
    {
        if (authorizationNames.IsNullOrEmpty())
        {

        }

        await Task.CompletedTask;
        return true;
    }
}