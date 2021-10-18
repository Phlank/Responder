using Newtonsoft.Json;
using System;

namespace Phlank.Responder
{
    public class NewtonsoftResponseConverter : JsonConverter<Response>
    {
        public override Response ReadJson(JsonReader reader, Type objectType, Response existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var serializableResponse = serializer.Deserialize<SerializableResponse>(reader);
            return serializableResponse.ToResponse();
        }

        public override void WriteJson(JsonWriter writer, Response value, JsonSerializer serializer)
        {
            var serializableResponse = value.ToSerializableResponse();
            serializer.Serialize(writer, serializableResponse, serializableResponse.GetType());
        }
    }

    public class NewtonsoftResponseConverter<T> : JsonConverter<Response<T>>
    {
        public override Response<T> ReadJson(JsonReader reader, Type objectType, Response<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var serializableResponse = serializer.Deserialize<SerializableResponse<T>>(reader);
            return serializableResponse.ToResponse<T>();
        }

        public override void WriteJson(JsonWriter writer, Response<T> value, JsonSerializer serializer)
        {
            var serializableResponse = value.ToSerializableResponse<T>();
            serializer.Serialize(writer, serializableResponse, serializableResponse.GetType());
        }
    }
}
