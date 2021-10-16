using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phlank.Responder
{
    internal class ResponseNewtonsoftJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Response);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var serializableResponse = serializer.Deserialize<SerializableResponse<object>>(reader);
            return serializableResponse.ToEmptyResponse();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var serializableResponse = ((Response)value).ToSerializableResponse();
            serializer.Serialize(writer, serializableResponse, serializableResponse.GetType());
        }
    }

    internal class ResponseNewtonsoftJsonConverter<T> : JsonConverter where T : class
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Response<T>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var serializableResponse = serializer.Deserialize<SerializableResponse<T>>(reader);
            return serializableResponse.ToResponse();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var serializableResponse = ((Response<T>)value).ToSerializableResponse();
            serializer.Serialize(writer, serializableResponse, serializableResponse.GetType());
        }
    }
}
