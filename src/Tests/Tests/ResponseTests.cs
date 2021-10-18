using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Phlank.Responder.Tests
{
    [TestClass]
    public class ResponseTests
    {
        [TestMethod]
        public void TestResponseProperties()
        {
            var response = new Response<string>();
            Assert.IsTrue(response.IsSuccessful);
            Assert.IsNull(response.Problem);
            Assert.IsTrue(response.Extensions.Count() == 0);
            
            response.Problem = new Problem(400);
            Assert.IsFalse(response.IsSuccessful);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.Problem.Status);

            response.Extensions.Add("TestKey", "TestValue");
            Assert.AreEqual("TestValue", response.Extensions["TestKey"]);

            response.Data = "TestData";
            Assert.AreEqual("TestData", response.Data);
        }

        //[TestMethod]
        //public void TestNewtonsoftSerialization()
        //{
        //    var successResponse = new Response<string>()
        //    {
        //        Data = "TestData",
        //        Extensions = new Dictionary<string, object>()
        //        {
        //            { "TestExtension1", "TestValue1" },
        //            { "TestExtension2", "TestValue2" }
        //        }
        //    };
        //    var successSerialized = JsonConvert.SerializeObject(successResponse, new NewtonsoftResponseConverter<string>());
        //    var successDeserialized = JsonConvert.DeserializeObject<Response<string>>(successSerialized, new NewtonsoftResponseConverter<string>());
        //    Assert.AreEqual("TestData", successDeserialized.Data);
        //    Assert.AreEqual("TestValue1", successDeserialized.Extensions["TestExtension1"]);
        //    Assert.AreEqual("TestValue2", successDeserialized.Extensions["TestExtension2"]);
        //    Assert.IsTrue(successDeserialized.IsSuccessful);

        //    var problemResponse = new Response<string>()
        //    {
        //        Data = "TestData",
        //        Extensions = new Dictionary<string, object>()
        //        {
        //            { "TestExtension1", "TestValue1" },
        //            { "TestExtension2", "TestValue2" }
        //        },
        //        Problem = new Problem(HttpStatusCode.BadRequest)
        //    };
        //    var problemSerialized = JsonConvert.SerializeObject(problemResponse);
        //    var problemDeserialized = JsonConvert.DeserializeObject<Response<string>>(problemSerialized);
        //    Assert.IsNull(problemDeserialized.Data);
        //    Assert.IsTrue(problemDeserialized.Extensions.Count() == 0);
        //    Assert.AreEqual(HttpStatusCode.BadRequest, problemDeserialized.Problem.Status);
        //    Assert.IsFalse(problemDeserialized.IsSuccessful);
        //}
    }
}
