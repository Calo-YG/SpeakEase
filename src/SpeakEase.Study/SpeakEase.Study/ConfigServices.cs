using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SpeakEase.Authorization.Authorization;
using SpeakEase.Infrastructure.Authorization;
using SpeakEase.Infrastructure.Options;
using SpeakEase.Infrastructure.Redis;
using SpeakEase.Study.Application.Auth;
using SpeakEase.Study.Application.User;
using SpeakEase.Study.Contract.Auth;
using SpeakEase.Study.Contract.Users;

namespace SpeakEase.Study.Host
{
    /// <summary>
    /// 基础服务构建
    /// </summary>
    public static class ConfigServices
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureJson(this IServiceCollection services)
        {
            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });
            return services;
        }

        /// <summary>
        /// 跨域配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureCors(this IServiceCollection services,IConfiguration configuration)
        {
            var cors = configuration.GetSection("CorsOptions").Get<CorsOptions>();

            services.AddCors(options =>options.AddPolicy(cors.Policy,builder =>
                builder
                    .WithOrigins(cors.Origins.Split(","))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials())
            );

            return services;
        }

        /// <summary>
        /// 配置jwt
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IServiceCollection ConfigureJwt(this IServiceCollection services,IConfiguration configuration)
        {
            var options = configuration.GetSection("JwtOptions").Get<JwtOptions>();
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            var secret = options.SecretKey;
            var issuser = options.Issuer;
            var audience = options.Audience;
            var expire = options.ExpMinutes;


            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(issuser) || string.IsNullOrEmpty(audience))
            {
                throw new Exception("validate jwt options failed");
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true, // 检查过期时间
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuser,
                        ValidAudience = audience,
                        ClockSkew = TimeSpan.FromSeconds(expire),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                    };
                });

            services.RegisterAuthorization();
            services.AddAuthorization();

            return services;
        }

        /// <summary>
        /// 配置redis
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureRedis(this IServiceCollection services,IConfiguration configuration)
        {

            services.Configure<RedisOptions>(configuration.GetSection("Redis"));

            services.AddSingleton<IRedisService, RedisService>();

            return services;
        }

        /// <summary>
        /// 注册应用服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection BuilderApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();    
            return services;
        }
    }
}
