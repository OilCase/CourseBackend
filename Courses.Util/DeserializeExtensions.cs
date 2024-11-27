using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Courses.Util.DeserializeExtensions;

public static class DeserializeExtensions
{
    private static JsonSerializerOptions defaultSerializerSettings =
        new JsonSerializerOptions();

    // set this up how you need to!
    private static JsonSerializerOptions featureXSerializerSettings =
        new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            NumberHandling = JsonNumberHandling.AllowReadingFromString |
                             JsonNumberHandling.WriteAsString,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin),
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            }
        };


    public static T Deserialize<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json, defaultSerializerSettings);
    }

    public static T DeserializeCustom<T>(this string json, JsonSerializerOptions settings)
    {
        return JsonSerializer.Deserialize<T>(json, settings);
    }

    public static T DeserializeFeatureX<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json, featureXSerializerSettings);
    }
}