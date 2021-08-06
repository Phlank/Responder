using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phlank.ApiModeling.Extensions;
using Phlank.ApiModeling.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace Phlank.ApiModeling.Tests
{
    [TestClass]
    public class ApiResultBuilderTests
    {
        private IApiResultBuilder _resultBuilder;
        private ApiWarning _warning;
        private ApiError _error;

        [TestInitialize]
        public void Setup()
        {
            IServiceCollection services = new ServiceCollection();
            services.ConfigureApiResponseBuilder();
            var provider = services.BuildServiceProvider();
            _resultBuilder = provider.GetRequiredService<IApiResultBuilder>();
            _warning = TestData.Warning;
            _error = TestData.Error;
        }

        [TestMethod]
        public void TestResponseWithNoOptions()
        {
            var result = _resultBuilder.Build();
            var response = (ApiResponse)result.Value;

            Assert.AreEqual(0, response.Warnings.Count());
            Assert.IsNull(response.Content);
        }

        [TestMethod]
        public void TestResponseFromSingleWarning()
        {
            _resultBuilder.WithWarning(_warning);
            var result = _resultBuilder.Build();
            var response = (ApiResponse)result.Value;
            var responseWarning = response.Warnings.First();

            Assert.AreEqual(_warning.Code, responseWarning.Code);
            Assert.AreEqual(_warning.Fields.First(), responseWarning.Fields.First());
            Assert.AreEqual(_warning.Message, responseWarning.Message);
            Assert.AreEqual(_warning.Severity, responseWarning.Severity);
        }

        [TestMethod]
        public void TestResponseFromWarningList()
        {
            _resultBuilder.WithWarnings(new List<ApiWarning> { _warning });
            var result = _resultBuilder.Build();
            var response = (ApiResponse)result.Value;
            var responseWarning = response.Warnings.First();

            Assert.AreEqual(_warning.Code, responseWarning.Code);
            Assert.AreEqual(_warning.Fields.First(), responseWarning.Fields.First());
            Assert.AreEqual(_warning.Message, responseWarning.Message);
            Assert.AreEqual(_warning.Severity, responseWarning.Severity);
        }

        [TestMethod]
        public void TestResponseFromSingleError()
        {
            _resultBuilder.WithError(_error);
            var result = _resultBuilder.Build();
            var response = (ApiError)result.Value;

            Assert.AreEqual(_error.Detail, response.Detail);
            Assert.AreEqual(_error.Title, response.Title);
            Assert.AreEqual(_error.Status, response.Status);
            Assert.AreEqual(_error.Type.OriginalString, response.Type.OriginalString);
            Assert.AreEqual(_error.Instance.OriginalString, response.Instance.OriginalString);
        }

        [TestMethod]
        public void TestResponseFromErrorList()
        {
            _resultBuilder.WithErrors(new List<ApiError> { _error });
            var result = _resultBuilder.Build();
            var response = (ApiError)result.Value;

            Assert.AreEqual(_error.Detail, response.Detail);
            Assert.AreEqual(_error.Title, response.Title);
            Assert.AreEqual(_error.Status, response.Status);
            Assert.AreEqual(_error.Type.OriginalString, response.Type.OriginalString);
            Assert.AreEqual(_error.Instance.OriginalString, response.Instance.OriginalString);
        }

        [TestMethod]
        public void TestEmptyResultIsSuccessful()
        {
            var result = _resultBuilder.Build();
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void TestExceptionWithUnsuccessfulCode()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _resultBuilder.WithStatusCodeOnSuccess(HttpStatusCode.Continue));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _resultBuilder.WithStatusCodeOnSuccess(HttpStatusCode.Redirect));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _resultBuilder.WithStatusCodeOnSuccess(HttpStatusCode.BadRequest));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _resultBuilder.WithStatusCodeOnSuccess(HttpStatusCode.InternalServerError));
        }

        [TestMethod]
        public void TestResultWithMultipleErrors()
        {
            _resultBuilder.WithError(TestData.Error);
            _resultBuilder.WithError(TestData.Error2);
            _resultBuilder.WithError(TestData.Error3);
            var result = _resultBuilder.Build();
            var json = JsonSerializer.Serialize(result.Value);
            var deserialized = JsonSerializer.Deserialize<ApiError>(json);
            var otherErrors = ((JsonElement)deserialized.Extensions["otherErrors"]).EnumerateArray();

            Assert.AreEqual(TestData.Error.Title, deserialized.Title);
            Assert.AreEqual(TestData.Error.Detail, deserialized.Detail);
            Assert.AreEqual(TestData.Error2.Title, otherErrors.First().GetProperty("title").GetString());
            Assert.AreEqual(TestData.Error2.Detail, otherErrors.First().GetProperty("detail").GetString());
            Assert.AreEqual(TestData.Error3.Title, otherErrors.Last().GetProperty("title").GetString());
            Assert.AreEqual(TestData.Error3.Detail, otherErrors.Last().GetProperty("detail").GetString());
        }

        [TestMethod]
        public void TestResultWithBasicException()
        {
            _resultBuilder.WithException(TestData.BasicException);
            var result = _resultBuilder.Build();
            var json = JsonSerializer.Serialize(result.Value);
            var deserialized = JsonSerializer.Deserialize<ApiError>(json);

            Assert.AreEqual(TestData.BasicException.Message, deserialized.Detail);
            Assert.AreEqual(TestData.BasicException.GetType().Name, deserialized.Title);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1", deserialized.Type.OriginalString);
            Assert.IsNull(deserialized.Instance);
        }

        [TestMethod]
        public void TestResultWithNotImplementedException()
        {
            _resultBuilder.WithException(TestData.NotImplementedException);
            var result = _resultBuilder.Build();
            var json = JsonSerializer.Serialize(result.Value);
            var deserialized = JsonSerializer.Deserialize<ApiError>(json);

            Assert.AreEqual(TestData.NotImplementedException.Message, deserialized.Detail);
            Assert.AreEqual(TestData.NotImplementedException.GetType().Name, deserialized.Title);
            Assert.AreEqual((int)HttpStatusCode.NotImplemented, result.StatusCode);
            Assert.AreEqual("https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.2", deserialized.Type.OriginalString);
            Assert.IsNull(deserialized.Instance);
        }

        [TestMethod]
        public void TestResultWithExceptionList()
        {
            _resultBuilder.WithExceptions(new List<Exception> { TestData.BasicException });
            var result = _resultBuilder.Build();
            var json = JsonSerializer.Serialize(result.Value);
            var deserialized = JsonSerializer.Deserialize<ApiError>(json);

            Assert.AreEqual(TestData.BasicException.Message, deserialized.Detail);
            Assert.AreEqual(TestData.BasicException.GetType().Name, deserialized.Title);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1", deserialized.Type.OriginalString);
            Assert.IsNull(deserialized.Instance);
        }
    }
}
