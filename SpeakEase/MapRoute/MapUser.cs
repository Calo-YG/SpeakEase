using SpeakEase.Contract.Users;
using SpeakEase.Contract.Users.Dto;
using SpeakEase.Infrastructure.Filters;

namespace SpeakEase.MapRoute
{
    /// <summary>
    /// User 路由映射
    /// </summary>
    public static class MapUser
    {
        /// <summary>
        /// 路由用户服务
        /// </summary>
        /// <param name="app"></param>
        public static void MapUserEndpoint(this IEndpointRouteBuilder app)
        {
            // 创建api 组
            var group = app.MapGroup("api/user")
                .WithDescription("用户服务")
                .WithTags("user")
                .AddEndpointFilter<ResultEndPointFilter>();

            //注册
            group.MapPost("Register", (IUserService userService,CreateUserInput request) =>
            {
                return userService.Register(request);

            }).WithSummary("注册");

            //修改
            group.MapPost("ModifyPassword", (IUserService userService,UpdateUserInput request) =>
            {
                return userService.ModifyPassword(request);

            }).WithSummary("修改密码").RequireAuthorization();

            //获取当前用户
            group.MapGet("CurrentUser", (IUserService userService) =>
            {
                return userService.CurrentUser();

            }).WithSummary("获取当前用户").RequireAuthorization();

            //上传头像
            group.MapPost("Uploadavatar", (IUserService userService,IFormFile file) =>
            {
                return userService.Uploadavatar(file);
            }).WithSummary("上传头像").RequireAuthorization().DisableAntiforgery();

            //创建用户设置请求
            group.MapPost("CreateUserSetting", (IUserService userService, UserSettingInput request) =>
            {
                return userService.CreateUserSetting(request);

            }).WithSummary("创建用户设置请求").RequireAuthorization();

            //更新用户设置请求
            group.MapPost("UpdateUserSetting", (IUserService userService, UserSettingUpdateInput request) =>
            {
                return userService.UpdateUserSetting(request);
            })
            .WithSummary("更新用户设置请求").RequireAuthorization();

            //获取当前用户设置
            group.MapGet("GetUserSetting", (IUserService userService) =>
            {
                return userService.GetUserSetting();
            }).WithSummary("获取当前用户设置").RequireAuthorization();

        }
    }
}
