using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpeakEase.Infrastructure.Shared;

public class DateTimeOffsetConverter:JsonConverter<DateTimeOffset>
{
      public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
      {
            return DateTime.Parse(reader.GetString());
      }

      public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
      {
            writer.WriteStringValue(value.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
      }
}
 
public class DateTimeOffsetNullableConverter : JsonConverter<DateTimeOffset?>
{
      public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
      {
            return string.IsNullOrEmpty(reader.GetString()) ? default(DateTime?) : DateTime.Parse(reader.GetString());
      }

      public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
      {
            writer.WriteStringValue(value?.ToString("yyyy-MM-dd HH:mm:ss"));
      }
}