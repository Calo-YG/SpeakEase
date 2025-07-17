using SpeakEase.Gateway.Contract.App.Dto;
using SpeakEase.Infrastructure.Shared;

namespace SpeakEase.Gateway.Contract.App;

/// <summary>
/// 应用服务
/// </summary>
public interface IAppService
{
    /// <summary>
    /// 创建应用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    Task CreateAppAsync(CreateAppInput input);

    /// <summary>
    /// 获取应用列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageResult<AppPageDto>> GetList(AppPageInput input);

    /// <summary>
    /// 获取应用详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<AppDto> GetAppById(string id);

    /// <summary>
    /// 修改应用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateAppAsync(UpdateAppInput input);

    /// <summary>
    /// 获取应用下拉列表    
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<List<AppSelectDto>> GetSelectAsync(string name);
}