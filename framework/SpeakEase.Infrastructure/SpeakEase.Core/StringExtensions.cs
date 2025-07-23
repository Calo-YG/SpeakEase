using System.Net;

namespace SpeakEase.Infrastructure.SpeakEase.Core;

public static class StringExtensions
{
    /// <summary>
    /// 移除指定后缀
    /// </summary>
    /// <param name="str"></param>
    /// <param name="postFixes"></param>
    /// <returns></returns>
    public static string RemoteFix(this string str, params string[] postFixes)
    {
        if (str == null)
        {
            return null;
        }

        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }

        if (postFixes is null)
        {
            return str;
        }

        foreach (string text in postFixes)
        {
            if (str.EndsWith(text))
            {
                return str.Left(str.Length - text.Length);
            }
        }

        return str;
    }


    private static string Left(this string str, int len)
    {
        if (str == null)
        {
            throw new ArgumentNullException("str");
        }

        if (str.Length < len)
        {
            throw new ArgumentException("len argument can not be bigger than given string's length!");
        }

        return str.Substring(0, len);
    }

    /// <summary>
    /// 字符串判空
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
    
    /// <summary>
    /// 添加http前缀
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string EnsureHttpPrefix(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return url;

        // 判断是否以 http:// 或 https:// 开头（忽略大小写）
        if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            // 没带协议，默认加 http://
            url = "http://" + url;
        }

        return url;
    }
    
    /// <summary>
    /// 专门判断IPv6地址
    /// </summary>
    public static bool IsIPv6Address(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        return IPAddress.TryParse(input, out IPAddress addr) && 
               addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;
    }
}