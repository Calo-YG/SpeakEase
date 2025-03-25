using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace SpeakEase.Infrastructure.Shared;

public class DateTimeOffsetConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // 从 JSON 读取字符串
        string dateString = reader.GetString();

        var tryconventerr = DateTime.TryParse(dateString, out var result);

        if (!tryconventerr)
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

public class DateTimeOffsetNullableConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string dateString = reader.GetString();

        if(dateString is null)
        {
            return null;
        }

        var tryconventerr = DateTime.TryParse(dateString, out var result);

        if (!tryconventerr)
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