using System.Text.Json;
using System.Text.Json.Serialization;

namespace Unit3dDescriptionClone.Serialization;

/// <summary>
/// Handles API responses where an integer field may be serialized as a JSON string (e.g. "12345") or as a number.
/// </summary>
internal sealed class NullableIntFromStringConverter : JsonConverter<int?>
{
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        if (reader.TokenType == JsonTokenType.Number)
            return reader.GetInt32();

        if (reader.TokenType == JsonTokenType.String)
        {
            var s = reader.GetString();
            if (string.IsNullOrEmpty(s))
                return null;
            if (int.TryParse(s, out var value))
                return value;
            return null;
        }

        throw new JsonException($"Cannot convert token type '{reader.TokenType}' to int?.");
    }

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
    {
        if (value is null)
            writer.WriteNullValue();
        else
            writer.WriteNumberValue(value.Value);
    }
}
