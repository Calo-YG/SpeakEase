using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SpeakEase.BuildingBlock.Grpc.BuildingBlock.Grpc;
using SpeakEase.BuildingBlock.Grpc.BuildingBlock.Grpc.Interface;
using SpeakEase.BuildingBlock.Grpc.BuildingBlock.Grpc.Options;

namespace SpeakEase.BuildingBlock.Grpc.Contrib.Grpc
{
    /// <summary>
    /// 默认 gRPC 服务构建工厂
    /// </summary>
    /// <param name="options">grppc</param>
    /// <param name="httpContextAccessor">httpContextAccessor</param>
    public class DefaultGrpcServiceBuildFactory(IOptions<List<GrpcServerOptions>> options, IHttpContextAccessor httpContextAccessor) :IGrpcServiceBuildFactory
    {
        /// <summary>
        /// grpc client options
        /// </summary>
        private readonly IReadOnlyList<GrpcServerOptions> _options = options.Value; 

        /// <summary>
        /// httpContextAccessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        /// <summary>
        /// 创建 gRPC 服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TService CreateGrpcService<TService>() where TService : IGrpcService
        {
            throw new NotImplementedException();
        }
    }
}
