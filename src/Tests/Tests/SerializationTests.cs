using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Phlank.Responder.Tests
{
    [TestClass]
    public class SerializationTests
    {
        private Newtonsoft.Json.JsonConverter<Response> _newtonsoftEmptyConverter;
        private Newtonsoft.Json.JsonConverter<Response<string>> _newtonsoftDataConverter;
        private System.Text.Json.Serialization.JsonConverter<Response> _systemEmptyConverter;
        private System.Text.Json.Serialization.JsonConverter<Response<string>> _systemDataConverter;
        private JsonSerializerOptions _systemOptions;

        [TestInitialize]
        public void Initialize()
        {
            _newtonsoftEmptyConverter = new NewtonsoftResponseConverter();
            _newtonsoftDataConverter = new NewtonsoftResponseConverter<string>();
            
            _systemEmptyConverter = new SystemTextJsonResponseConverter();
            _systemDataConverter = new SystemTextJsonResponseConverter<string>();

            _systemOptions = new JsonSerializerOptions();
            _systemOptions.Converters.Add(_systemEmptyConverter);
            _systemOptions.Converters.Add(_systemDataConverter);
        }

        [TestMethod]
        public void TestNewtonsoftSerializationSuccessWithoutData()
        {
            var response = new Response()
            {
                Extensions = new Dictionary<string, object>()
                {
                    { "TestExtensionKey", "TestExtensionValue" }
                }
            };
            var serialized = JsonConvert.SerializeObject(response, _newtonsoftEmptyConverter);
            var deserialized = JsonConvert.DeserializeObject<Response>(serialized, _newtonsoftEmptyConverter);

            Assert.IsTrue(deserialized.IsSuccessful);
            Assert.AreEqual("TestExtensionValue", deserialized.Extensions["TestExtensionKey"]);
            Assert.IsNull(deserialized.Problem);
        }

        [TestMethod]
        public void TestNewtonsoftSerializationFailureWithoutData()
        {
            var response = new Response()
            {
                Problem = new Problem(HttpStatusCode.BadRequest),
                Extensions = new Dictionary<string, object>()
                {
                    { "TestExtensionKey", "TestExtensionValue" }
                }
            };
            var serialized = JsonConvert.SerializeObject(response, _newtonsoftEmptyConverter);
            var deserialized = JsonConvert.DeserializeObject<Response>(serialized, _newtonsoftEmptyConverter);

            Assert.IsFalse(deserialized.IsSuccessful);
            Assert.IsTrue(deserialized.Extensions.Count() == 0);
            Assert.AreEqual(HttpStatusCode.BadRequest, deserialized.Problem.Status);
        }

        [TestMethod]
        public void TestNewtonsoftSerializationSuccessWithData()
        {
            var response = new Response<string>()
            {
                Data = "TestData",
                Extensions = new Dictionary<string, object>()
                {
                    { "TestExtensionKey", "TestExtensionValue" }
                }
            };
            var serialized = JsonConvert.SerializeObject(response, _newtonsoftDataConverter);
            var deserialized = JsonConvert.DeserializeObject<Response<string>>(serialized, _newtonsoftDataConverter);

            Assert.IsTrue(deserialized.IsSuccessful);
            Assert.AreEqual("TestExtensionValue", deserialized.Extensions["TestExtensionKey"]);
            Assert.IsNull(deserialized.Problem);
            Assert.AreEqual("TestData", deserialized.Data);
        }

        [TestMethod]
        public void TestNewtonsoftSerializationFailureWithData()
        {
            var successfulResponse = new Response<string>()
            {
                Problem = new Problem(HttpStatusCode.BadRequest),
                Extensions = new Dictionary<string, object>()
                {
                    { "TestExtensionKey", "TestExtensionValue" }
                },
                Data = "TestData"
            };
            var successSerialized = JsonConvert.SerializeObject(successfulResponse, _newtonsoftDataConverter);
            var successDeserialized = JsonConvert.DeserializeObject<Response<string>>(successSerialized, _newtonsoftDataConverter);

            Assert.IsFalse(successDeserialized.IsSuccessful);
            Assert.IsTrue(successDeserialized.Extensions.Count() == 0);
            Assert.AreEqual(HttpStatusCode.BadRequest, successDeserialized.Problem.Status);
            Assert.IsNull(successDeserialized.Data);
        }

        [TestMethod]
        public void TestSystemTextJsonSerializationSuccessWithoutData()
        {
            var successfulResponse = new Response()
            {
                Extensions = new Dictionary<string, object>()
                {
                    { "TestExtensionKey", "TestExtensionValue" }
                }
            };
            var successSerialized = System.Text.Json.JsonSerializer.Serialize(successfulResponse, _systemOptions);
            var successDeserialized = System.Text.Json.JsonSerializer.Deserialize<Response>(successSerialized, _systemOptions);

            Assert.IsTrue(successDeserialized.IsSuccessful);
            Assert.AreEqual("TestExtensionValue", successDeserialized.Extensions["TestExtensionKey"].ToString());
            Assert.IsNull(successDeserialized.Problem);
        }

        [TestMethod]
        public void TestSystemTextJsonSerializationFailureWithoutData()
        {
            var successfulResponse = new Response()
            {
                Problem = new Problem(HttpStatusCode.BadRequest),
                Extensions = new Dictionary<string, object>()
                {
                    { "TestExtensionKey", "TestExtensionValue" }
                }
            };
            var successSerialized = JsonConvert.SerializeObject(successfulResponse, _newtonsoftEmptyConverter);
            var successDeserialized = JsonConvert.DeserializeObject<Response>(successSerialized, _newtonsoftEmptyConverter);

            Assert.IsFalse(successDeserialized.IsSuccessful);
            Assert.IsTrue(successDeserialized.Extensions.Count() == 0);
            Assert.AreEqual(HttpStatusCode.BadRequest, successDeserialized.Problem.Status);
        }

        [TestMethod]
        public void TestSystemTextJsonSerializationSuccessWithData()
        {
            var successfulResponse = new Response<string>()
            {
                Data = "TestData",
                Extensions = new Dictionary<string, object>()
                {
                    { "TestExtensionKey", "TestExtensionValue" }
                }
            };
            var successSerialized = JsonConvert.SerializeObject(successfulResponse, _newtonsoftDataConverter);
            var successDeserialized = JsonConvert.DeserializeObject<Response<string>>(successSerialized, _newtonsoftDataConverter);

            Assert.IsTrue(successDeserialized.IsSuccessful);
            Assert.AreEqual("TestExtensionValue", successDeserialized.Extensions["TestExtensionKey"]);
            Assert.IsNull(successDeserialized.Problem);
            Assert.AreEqual("TestData", successDeserialized.Data);
        }

        [TestMethod]
        public void TesSystemTextJsonSerializationFailureWithData()
        {
            var successfulResponse = new Response<string>()
            {
                Problem = new Problem(HttpStatusCode.BadRequest),
                Extensions = new Dictionary<string, object>()
                {
                    { "TestExtensionKey", "TestExtensionValue" }
                },
                Data = "TestData"
            };
            var successSerialized = JsonConvert.SerializeObject(successfulResponse, _newtonsoftDataConverter);
            var successDeserialized = JsonConvert.DeserializeObject<Response<string>>(successSerialized, _newtonsoftDataConverter);

            Assert.IsFalse(successDeserialized.IsSuccessful);
            Assert.IsTrue(successDeserialized.Extensions.Count() == 0);
            Assert.AreEqual(HttpStatusCode.BadRequest, successDeserialized.Problem.Status);
            Assert.IsNull(successDeserialized.Data);
        }
    }
}
