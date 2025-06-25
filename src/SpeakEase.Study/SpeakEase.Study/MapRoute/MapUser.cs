using SpeakEase.Infrastructure.Filters;
using SpeakEase.Study.Contract.Users;
using SpeakEase.Study.Contract.Users.Dto;

namespace SpeakEase.Study.Host.MapRoute
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
            group.MapPost("register", (IUserService userService,CreateUserInput request) => userService.Register(request)).WithSummary("注册");

            //修改
            group.MapPost("modifyPassword", (IUserService userService,UpdateUserInput request) => userService.ModifyPassword(request)).WithSummary("修改密码").RequireAuthorization();

            //获取当前用户
            group.MapGet("currentUser", (IUserService userService) => userService.CurrentUser()).WithSummary("获取当前用户").RequireAuthorization();

            //上传头像
            group.MapPost("uploadAvatar", (IUserService userService,IFormFile file) => userService.Uploadavatar(file)).WithSummary("上传头像").RequireAuthorization().DisableAntiforgery();

            //创建用户设置请求
            group.MapPost("createUserSetting", (IUserService userService, UserSettingInput request) => userService.CreateUserSetting(request)).WithSummary("创建用户设置请求").RequireAuthorization();

            //更新用户设置请求
            group.MapPost("updateUserSetting", (IUserService userService, UserSettingUpdateInput request) => userService.UpdateUserSetting(request))
            .WithSummary("更新用户设置请求").RequireAuthorization();

            //获取当前用户设置
            group.MapGet("getUserSetting", (IUserService userService) => userService.GetUserSetting()).WithSummary("获取当前用户设置").RequireAuthorization();

        }
    }
}
