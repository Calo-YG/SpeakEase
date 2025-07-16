using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using SpeakEase.Gateway.Infrastructure.Yarp.Core.BuildBlock;

namespace SpeakEase.Gateway.Infrastructure.Yarp.Core.Contrib;

public class ProxyConfigDataBaseProvider(IDbContext dbContext):IProxyConfigDataBaseProvider
{
    
}