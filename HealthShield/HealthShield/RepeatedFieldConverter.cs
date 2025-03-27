using Google.Protobuf.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthShield;

public class RepeatedFieldJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(RepeatedField<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type elementType = typeToConvert.GetGenericArguments()[0];
        Type converterType = typeof(RepeatedFieldConverter<>).MakeGenericType(elementType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

public class RepeatedFieldConverter<T> : JsonConverter<RepeatedField<T>>
{
    public override RepeatedField<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var list = JsonSerializer.Deserialize<List<T>>(ref reader, options);
        var repeatedField = new RepeatedField<T>();
        if (list != null) repeatedField.AddRange(list);
        return repeatedField;
    }

    public override void Write(Utf8JsonWriter writer, RepeatedField<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.ToList(), options);
    }
}

