using SpeakEase.Infrastructure.SpeakEase.Core;
using Yitter.IdGenerator;

namespace SpeakEase.Infrastructure.WorkIdGenerate;

public class IdGenerate :IIdGenerate
{
    
    public IdGenerate(IWorkIdGenerate workIdGenerate)
    {
        var options = workIdGenerate.Options;
        YitIdHelper.SetIdGenerator(new IdGeneratorOptions
        {
            WorkerId = workIdGenerate.GetWorkId(),  
            MaxSeqNumber = options.MaxSeqNumber,
            MinSeqNumber = options.MinSeqNumber,  
            WorkerIdBitLength = options.WorkerIdBitLength,
        });
    }
    
    /// <summary>
    /// 生成Id
    /// </summary>
    /// <returns></returns>
    public long NewId()
    {
        return YitIdHelper.NextId();
    }

    /// <summary>
    /// 生成id
    /// </summary>
    /// <returns></returns>
    public string NewIdString()
    {
        return LongToStringConverter.Convert(YitIdHelper.NextId());
    }
}