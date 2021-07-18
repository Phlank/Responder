using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phlank.ApiModeling.Extensions;
using Phlank.ApiModeling.Tests.Data;
using System.Collections.Generic;
using System.Linq;

namespace Phlank.ApiModeling.Tests.Tests
{
    [TestClass]
    public class ApiResponseBuilderTests
    {
        private IApiResponseBuilder _responseBuilder;
        private ApiWarning _warning;
        private ApiError _error;

        [TestInitialize]
        public void Setup()
        {
            IServiceCollection services = new ServiceCollection();
            services.ConfigureApiResponseBuilder();
            var provider = services.BuildServiceProvider();
            _responseBuilder = provider.GetRequiredService<IApiResponseBuilder>();
            _warning = TestData.Warning;
            _error = TestData.Error;
        }

        [TestMethod]
        public void TestResponseWithNoOptions()
        {
            var response = _responseBuilder.Build();

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Errors);
            Assert.IsNull(response.Warnings);
        }

        [TestMethod]
        public void TestResponseFromSingleWarning()
        {
            _responseBuilder.WithWarning(_warning);
            var response = _responseBuilder.Build();
            var responseWarning = response.Warnings.First();

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Errors);
            Assert.AreEqual(_warning.Code, responseWarning.Code);
            Assert.AreEqual(_warning.Fields.First(), responseWarning.Fields.First());
            Assert.AreEqual(_warning.Message, responseWarning.Message);
            Assert.AreEqual(_warning.Severity, responseWarning.Severity);
        }

        [TestMethod]
        public void TestResponseFromWarningList()
        {
            _responseBuilder.WithWarnings(new List<ApiWarning> { _warning });
            var response = _responseBuilder.Build();
            var responseWarning = response.Warnings.First();

            Assert.IsTrue(response.Success);
            Assert.IsNull(response.Errors);
            Assert.AreEqual(_warning.Code, responseWarning.Code);
            Assert.AreEqual(_warning.Fields.First(), responseWarning.Fields.First());
            Assert.AreEqual(_warning.Message, responseWarning.Message);
            Assert.AreEqual(_warning.Severity, responseWarning.Severity);
        }

        [TestMethod]
        public void TestResponseFromSingleError()
        {
            _responseBuilder.WithError(_error);
            var response = _responseBuilder.Build();
            var responseError = response.Errors.First();

            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Warnings);
            Assert.AreEqual(_error.Code, responseError.Code);
            Assert.AreEqual(_error.Fields.First(), responseError.Fields.First());
            Assert.AreEqual(_error.Message, responseError.Message);
        }

        [TestMethod]
        public void TestResponseFromErrorList()
        {
            _responseBuilder.WithErrors(new List<ApiError> { _error });
            var response = _responseBuilder.Build();
            var responseError = response.Errors.First();

            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Warnings);
            Assert.AreEqual(_error.Code, responseError.Code);
            Assert.AreEqual(_error.Fields.First(), responseError.Fields.First());
            Assert.AreEqual(_error.Message, responseError.Message);
        }
    }
}
