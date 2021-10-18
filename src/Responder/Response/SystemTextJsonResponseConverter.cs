using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Phlank.Responder
{
    public class SystemTextJsonResponseConverter : JsonConverter<Response>
    {
        public override Response Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var serializableResponse = JsonSerializer.Deserialize<SerializableResponse>(ref reader, options);
            return serializableResponse.ToResponse();
        }

        public override void Write(Utf8JsonWriter writer, Response value, JsonSerializerOptions options)
        {
            var serializableResponse = value.ToSerializableResponse();
            JsonSerializer.Serialize(writer, serializableResponse, options);
        }
    }

    public class SystemTextJsonResponseConverter<T> : JsonConverter<Response<T>>
    {
        public override Response<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var serializableResponse = JsonSerializer.Deserialize<SerializableResponse<T>>(ref reader, options);
            return serializableResponse.ToResponse();
        }

        public override void Write(Utf8JsonWriter writer, Response<T> value, JsonSerializerOptions options)
        {
            var serializableResponse = value.ToSerializableResponse();
            JsonSerializer.Serialize(writer, serializableResponse, options);
        }
    }
}