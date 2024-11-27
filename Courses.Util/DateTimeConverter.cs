using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Courses.Util;
public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        string dateStr = reader.GetString();
        return DateTime.ParseExact(dateStr, "dd.MM.yyyy", null).ToUniversalTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        string formattedDate = value.ToString("dd.MM.yyyy");
        writer.WriteStringValue(formattedDate);
    }
}