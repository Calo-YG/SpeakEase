using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpeakEase.Infrastructure.Redis;

namespace SpeakEase.Infrastructure.WorkIdGenerate;

/// <summary>
/// 工作Id生成  
/// </summary>
public class WorkIdGenerate : BackgroundService,IWorkIdGenerate
{
    /// <summary>
    /// workid
    /// </summary>
    private readonly ushort? _workId;

    private readonly IOptionsSnapshot<WorkIdGenerateOptions> _options;
    private readonly IRedisService _redisService;
    private readonly ILogger<WorkIdGenerate> _logger;
    private readonly string _workName;

    /// <summary>
    /// 工作Id生成  
    /// </summary>
    public WorkIdGenerate(IServiceProvider serviceProvider,IRedisService redisService,ILogger<WorkIdGenerate> logger)
    {
        _options = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IOptionsSnapshot<WorkIdGenerateOptions>>();
        _redisService = redisService;
        _logger = logger;
        _workName = _options.Value.AppName + Guid.NewGuid().ToString("N");
        _workId = GenerateWorkId().GetAwaiter().GetResult();
    }

    public WorkIdGenerateOptions Options => _options.Value;
    
    public ushort GetWorkId()
    {
        if (!_workId.HasValue)
        {
            _logger.LogError("workId");
            throw new ArgumentException("workId");
        }
        
        return _workId.Value;
    }
    
    
    /// <summary>
    /// 获取workid redis key
    /// </summary>
    /// <param name="workId"></param>
    /// <returns></returns>
    private string GetWorkerIdKey(int workId)
    {
        if (string.IsNullOrEmpty(Options.RedisKeyPrefix))
        {
            return $"workId:{Options.AppName}:{workId}";
        }

        return $"{Options.RedisKeyPrefix}:workId:{Options.AppName}:{workId}";
    }

    /// <summary>
    /// 获取workid
    /// </summary>
    /// <returns></returns>
    private async Task<ushort> GenerateWorkId()
    {
        var workIdKey = GetWorkerIdKey(_workId ?? 0);
        
        var value =  _redisService.Get<string>(workIdKey);

        if (_workId.HasValue && string.IsNullOrEmpty(value) && value != _workName)
        {
            return _workId.Value;
        }

        _logger.LogInformation("workId");
        
        // 计算最大值，确保不会超过 ushort.MaxValue
        var maxBitValue = (int)Math.Pow(2, Options.WorkerIdBitLength);
        if (maxBitValue > ushort.MaxValue)
        {
            _logger.LogWarning("WorkerIdBitLength 超过 ushort 最大限制，已限制为 16bit");
            maxBitValue = ushort.MaxValue + 1; // 注意 Random.Next 是 [min, max)
        }

        ushort resultWorkId = 0;

        while (true)
        {
            var current = (ushort)Random.Shared.Next(1, maxBitValue-1); // 从 1 开始避免为 0

            workIdKey = GetWorkerIdKey(current);

            var success = await _redisService.SetNxAsync(workIdKey, _workName, Options.SessionRefreshIntervalSeconds);
            
            if (success)
            {
                resultWorkId = current;
                break;
            }
        }
        _logger.LogInformation($"workId:{resultWorkId}");
        return resultWorkId;
    }

    /// <summary>
    /// 获取workId
    /// </summary>
    /// <param name="stoppingToken"></param>
    protected  override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(Options.SessionRefreshIntervalSeconds * 1000,stoppingToken);
            
            await GenerateWorkId();
        }
    }
    

    /// <summary>
    /// 停止
    /// </summary>
    /// <param name="cancellationToken"></param>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        var workIdKey = GetWorkerIdKey(_workId!.Value);

        await _redisService.DeleteAsync(workIdKey);
        
        await base.StopAsync(cancellationToken);
    }
}