using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpeakEase.Gateway.Domain.Entity.System;
using SpeakEase.Gateway.Infrastructure.EntityFrameworkCore;
using System.Threading.Channels;

namespace SpeakEase.Gateway.Infrastructure.GatewayLog
{
    internal class OpereateLogBackgroundService : BackgroundService, IOperateLog
    {
        private readonly Channel<OperateLog> OperateLogChannel;

        private readonly ChannelReader<OperateLog> OperateLogReader;

        private readonly ChannelWriter<OperateLog> OperateLogWriter;

        private readonly IServiceProvider _serviceProvider;

        public OpereateLogBackgroundService(IServiceProvider serviceProvider)
        {
            OperateLogChannel = Channel.CreateBounded<OperateLog>(1000);
            OperateLogReader = OperateLogChannel.Reader;
            OperateLogWriter = OperateLogChannel.Writer;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 执行日志入库
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override  Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                while (OperateLogReader.TryRead(out var item))
                {
                    using var scope = _serviceProvider.CreateScope();

                    var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

                    var sysLog = new SysLogEntity
                    {

                    };

                    dbContext.SysLog.Add(sysLog);
                    dbContext.SaveChanges();
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="operate"></param>
        /// <returns></returns>
        public async Task WriteLogAsync(OperateLog operate)
        {
            while (await OperateLogWriter.WaitToWriteAsync())
            {
                await OperateLogWriter.WriteAsync(operate);

                break;
            }
        }
    }
}
