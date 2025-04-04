﻿using System.Text;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace SpeakEase.Infrastructure.SpeakEase.Core;

public class SerilogRequestUtility
{
    public const string HttpMessageTemplate =
        "RequestIp:{RequestIp}  HTTP {RequestMethod} {RequestPath} QueryString:{QueryString} Body:{Body}  responded {StatusCode} in {Elapsed:0.0000} ms  SpeakEase";


    public static LogEventLevel GetRequestLevel(HttpContext ctx, double _, Exception ex) =>
        ex is null && ctx.Response.StatusCode <= 499 ? IgnoreRequest(ctx) : LogEventLevel.Error;

    private static LogEventLevel IgnoreRequest(HttpContext ctx)
    {
        var path = ctx.Request.Path.Value;

        return LogEventLevel.Information;
    }

    /// <summary>
    /// 从Request中增加附属属性
    /// </summary>
    /// <param name="diagnosticContext"></param>
    /// <param name="httpContext"></param>
    public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
    {
        var request = httpContext.Request;

        diagnosticContext.Set("RequestHost", request.Host);
        diagnosticContext.Set("RequestScheme", request.Scheme);
        diagnosticContext.Set("Protocol", request.Protocol);
        diagnosticContext.Set("RequestIp", httpContext.GetRequestIp());

        if (request.Method == HttpMethods.Get)
        {
            diagnosticContext.Set("QueryString", request.QueryString.HasValue ? request.QueryString.Value : string.Empty);
            diagnosticContext.Set("Body", string.Empty);
        }
        else
        {
            diagnosticContext.Set("QueryString", request.QueryString.HasValue ? request.QueryString.Value : string.Empty);
            diagnosticContext.Set("Body", request.ContentLength > 0 ? request.GetRequestBody() : string.Empty);
        }

        diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

        if (request.Path.HasValue)
        {
            diagnosticContext.Set("RequestPath", request.Path.Value);
        }
    }
}

public static class HttpRequestExtensions
{
    public static string GetRequestBody(this HttpRequest request)
    {
        request.EnableBuffering();

        var body = request.Body;

        if (body is null)
        {
            return null;
        }

        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        _ = body!.Read(buffer, 0, buffer.Length);
        var bodyAsText = Encoding.UTF8.GetString(buffer);
        body.Seek(0, SeekOrigin.Begin);
        request.Body = body;
        return bodyAsText;
    }
}