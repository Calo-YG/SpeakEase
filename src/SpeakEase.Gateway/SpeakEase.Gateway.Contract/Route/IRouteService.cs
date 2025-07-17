using SpeakEase.Gateway.Contract.Route.Dto;
using SpeakEase.Infrastructure.Shared;

namespace SpeakEase.Gateway.Contract.Route;

/// <summary>
/// 路由服务
/// </summary>
public interface IRouteService
{
    /// <summary>
    /// 创建路由
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task CreateRouteAsync(CreateRouteInput input);

    /// <summary>
    /// 修改路由
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateRouteAsync(UpdateRouteInput input);

    /// <summary>
    /// 获取路由列表
    /// </summary>
    /// <returns></returns>
    Task<PageResult<RoutePageDto>> GetListAsync(RoutePageInput input);
    
    /// <summary>
    /// 获取路由详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<RouteDto> GetByIdAsync(string id);

    /// <summary>
    /// 删除路由
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(string id);
}