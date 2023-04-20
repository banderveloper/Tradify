using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tradify.Identity.Application.Common.Converters;

public class SnakeCaseStringEnumConverter<T> : JsonConverter<T>
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
        var snakeCaseValue = ToSnakeCase(value.ToString());
        writer.WriteStringValue(snakeCaseValue);
    }

    private static string ToSnakeCase(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        var result = new StringBuilder();
        for (var i = 0; i < value.Length; i++)
        {
            if (i > 0 && char.IsUpper(value[i]))
            {
                result.Append('_');
            }
            result.Append(char.ToLower(value[i]));
        }
        return result.ToString();
    }
}