using SpeakEase.Infrastructure.Filters;
using SpeakEase.Study.Contract.Auth;
using SpeakEase.Study.Contract.Auth.Dto;

namespace SpeakEase.Study.Host.MapRoute
{
    /// <summary>
    /// Auth 路由映射
    /// </summary>
    public static class MapAuth 
    {
        /// <summary>
        /// 路由 Auth 
        /// </summary>
        /// <param name="app"></param>
        public static void MapAuthEndpoint(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/auth")
                .WithDescription("授权鉴权")
                .WithTags("auth")
                .AddEndpointFilter<ResultEndPointFilter>();

            //获取验证码
            group.MapGet("getVerificationCode", (IAuthService authService,string capcha) => authService.GetVerificationCode(capcha)).WithSummary("获取验证码");

            //登录
            group.MapPost("login", (IAuthService authService, LoginInput request) => authService.Login(request)).WithSummary("登录");

            //退出登录
            group.MapPost("loginOut", (IAuthService authService) => authService.LoginOut())
             .WithSummary("退出登录")
             .RequireAuthorization();

            //刷新token
            group.MapPost("refreshToken", (IAuthService authService, RefreshTokenInput request) => authService.RefreshToken(request))
            .WithSummary("refreshToken");
        }
    }
}
