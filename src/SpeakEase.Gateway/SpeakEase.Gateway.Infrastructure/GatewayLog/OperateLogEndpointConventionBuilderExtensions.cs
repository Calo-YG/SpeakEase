using Microsoft.AspNetCore.Builder;

namespace SpeakEase.Gateway.Infrastructure.GatewayLog
{
    public static class OperateLogEndpointConventionBuilderExtensions
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="logOperate"></param>
        private static void RequireOperateLogCore<TBuilder>(TBuilder builder,ILogOperate logOperate) where TBuilder : IEndpointConventionBuilder
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentNullException.ThrowIfNull(logOperate, nameof(logOperate));

            builder.Add(endpointBuilder =>
            {
                endpointBuilder.Metadata.Add(logOperate);
            });
        }

        /// <summary>
        /// 日志记录器
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="module"></param>
        /// <param name="route"></param>
        public static void RequireOperateLog<TBuilder>(this TBuilder builder, string module,string route) where TBuilder : IEndpointConventionBuilder
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentException.ThrowIfNullOrEmpty(module, nameof(module));
            ArgumentNullException.ThrowIfNullOrEmpty(route, nameof(route));
            RequireOperateLogCore(builder,new OperateLogAttribute(module,route));
        }

        /// <summary>
        ///  日志记录器写入数据库
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <param name="builder"></param>
        /// <param name="module"></param>
        /// <param name="route"></param>
        public static void RequireOperateLogDataBase<TBuilder>(this TBuilder builder,string module,string route) where TBuilder : IEndpointConventionBuilder
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            ArgumentException.ThrowIfNullOrEmpty(module, nameof(module));
            ArgumentNullException.ThrowIfNullOrEmpty(route, nameof(route));
            RequireOperateLogCore(builder, new OperateLogAttribute(module, route,true));
        }
    }
}
