namespace SpeakEase.Infrastructure.Authorization;

public class PermissionCheck : IPermissionCheck
{
    public async Task<bool> IsGranted(string userId, string[] authorizationNames)
    {
        await Task.CompletedTask;
        return true;
    }
}