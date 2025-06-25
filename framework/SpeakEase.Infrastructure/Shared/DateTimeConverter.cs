using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpeakEase.Infrastructure.Shared;

/// <summary>
/// 时间转换
/// </summary>
public class DateTimeOffsetConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // 从 JSON 读取字符串
        var dateString = reader.GetString();

        var converter = DateTime.TryParse(dateString, out var result);

        if (!converter)
        {
            throw new JsonException($"无法将 \"{dateString}\" 解析为 DateTime");
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}

/// <summary>
/// 时间转换
/// </summary>
public class DateTimeOffsetNullableConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();

        if(dateString is null)
        {
            return null;
        }

        var converter = DateTime.TryParse(dateString, out var result);

        if (!converter)
        {
            throw new JsonException($"无法将 \"{dateString}\" 解析为 DateTime");
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}