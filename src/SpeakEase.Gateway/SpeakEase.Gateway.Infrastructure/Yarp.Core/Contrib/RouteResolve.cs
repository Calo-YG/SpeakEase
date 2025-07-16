using Microsoft.AspNetCore.Http;
using SpeakEase.Gateway.Infrastructure.Yarp.Core.BuildBlock;

namespace SpeakEase.Gateway.Infrastructure.Yarp.Core.Contrib;

public class RouteResolve(IHttpContextAccessor httpContextAccessor):IRouteResolve
{
    
}