﻿using SpeakEase.Infrastructure.SpeakEase.Core;

namespace SpeakEase.Infrastructure.Authorization;

public class PermissionCheck : IPermissionCheck
{
    public async Task<bool> IsGranted(long userId, string authorizationNames)
    {
        if (authorizationNames.IsNullOrEmpty())
        {

        }

        await Task.CompletedTask;
        return true;
    }
}