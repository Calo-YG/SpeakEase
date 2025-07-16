using SpeakEase.Gateway.Contract.App.Dto;
using SpeakEase.Infrastructure.Shared;

namespace SpeakEase.Gateway.Contract.App;

/// <summary>
/// 应用服务
/// </summary>
public interface IAppService
{
    Task CreateAppAsync(CreateAppInput input);

    Task<PageResult<AppPageDto>> GetList(AppPageInput input);
}