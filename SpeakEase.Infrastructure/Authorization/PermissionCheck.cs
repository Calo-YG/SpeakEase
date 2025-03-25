namespace SpeakEase.Infrastructure.Authorization;

public class PermissionCheck : IPermissionCheck
{
    public async Task<bool> IsGranted(long userId, string[] authorizationNames)
    {
        await Task.CompletedTask;
        return true;
    }
}