using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tradify.Identity.Application.Common.Converters;

public class JsonStringEnumConverter<T> : JsonConverter<T> 
    where T : struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return Enum.Parse<T>(reader.GetString(), ignoreCase: true);
        }
        catch (Exception ex)
        {
            throw new JsonException($"Error deserializing {typeToConvert.Name}: {ex.Message}");
        }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}