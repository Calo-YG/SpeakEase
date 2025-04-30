using SpeakEase.Contract.Auth;
using SpeakEase.Contract.Auth.Dto;
using SpeakEase.Infrastructure.Filters;

namespace SpeakEase.MapRoute
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
        public static void MapAuthEndponit(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/auth")
                .WithDescription("授权鉴权")
                .WithTags("auth")
                .AddEndpointFilter<ResultEndPointFilter>();

            //获取验证码
            group.MapGet("GetVerificationCode", (IAuthService authService, string capcha) =>
            {
                return authService.GetVerificationCode(capcha);
            }).WithSummary("获取验证码");

            //登录
            group.MapPost("login", (IAuthService authService, LoginRequest request) =>
            {
                return authService.Login(request);
            }).WithSummary("登录");

            //退出登录
            group.MapPost("loginout", (IAuthService authService) =>
            {
                return authService.LoginOut();
            })
             .WithSummary("退出登录")
             .RequireAuthorization();

            //刷新token
            group.MapPost("refreshToken", (IAuthService authService, RefreshTokenRequest request) =>
            {
                return authService.RefreshToken(request);
            })
            .WithSummary("refreshToken");
        }
    }
}
