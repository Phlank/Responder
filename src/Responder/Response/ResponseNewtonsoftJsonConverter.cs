using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phlank.Responder
{
    internal class ResponseNewtonsoftJsonConverter<T> : JsonConverter where T : class
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Response<T>)) return true;
            else return false;
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
