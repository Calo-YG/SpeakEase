namespace SpeakEase.Infrastructure.SpeakEase.Core;

/// <summary>
/// 将 long 类型转换为字符串的工具类
/// </summary>
public class LongToStringConverter
{
    private const char ZeroChar = '0';

    public static string Convert(long value)
    {
        // 处理最小值（long.MinValue 无法直接取绝对值）
        if (value == long.MinValue)
            return "-9223372036854775808";

        // 计算字符缓冲区长度（最大 20 字符：符号位 + 19 位数字）
        Span<char> buffer = stackalloc char[20];
        int index = buffer.Length;

        bool isNegative = value < 0;
        ulong number = isNegative ? (ulong)(-value) : (ulong)value;

        // 逐位填充字符（从后向前）
        do
        {
            buffer[--index] = (char)(number % 10 + ZeroChar);
            number /= 10;
        } while (number > 0);

        // 添加负号
        if (isNegative)
            buffer[--index] = '-';

        // 返回有效部分
        return buffer.Slice(index).ToString();
    }
}