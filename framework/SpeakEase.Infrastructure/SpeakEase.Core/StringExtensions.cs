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
}