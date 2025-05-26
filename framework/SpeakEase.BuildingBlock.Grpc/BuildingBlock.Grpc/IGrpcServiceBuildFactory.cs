using SpeakEase.BuildingBlock.Grpc.BuildingBlock.Grpc.Interface;

namespace SpeakEase.BuildingBlock.Grpc.BuildingBlock.Grpc
{
    /// <summary>
    /// Gprc 服务构建工厂接口
    /// </summary>
    public interface IGrpcServiceBuildFactory
    {
        /// <summary>
        /// 创建 gRPC 服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        TService CreateGrpcService<TService>() where TService : IGrpcService;
    }
}
