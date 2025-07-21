namespace SpeakEase.Infrastructure.WorkIdGenerate;

public interface IIdGenerate
{
    /// <summary>
    /// 生成Id
    /// </summary>
    /// <returns></returns>
    public long NewId();

    /// <summary>
    /// 生成id
    /// </summary>
    /// <returns></returns>
    public string NewIdString();
}