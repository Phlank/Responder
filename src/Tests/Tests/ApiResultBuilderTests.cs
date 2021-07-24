using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phlank.ApiModeling.Extensions;
using Phlank.ApiModeling.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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

            Assert.IsTrue(response.Success);
            Assert.AreEqual(0, response.Warnings.Count());
            Assert.AreEqual(0, response.Errors.Count());
        }

        [TestMethod]
        public void TestResponseFromSingleWarning()
        {
            _resultBuilder.WithWarning(_warning);
            var result = _resultBuilder.Build();
            var response = (ApiResponse)result.Value;
            var responseWarning = response.Warnings.First();

            Assert.IsTrue(response.Success);
            Assert.AreEqual(0, response.Errors.Count());
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

            Assert.IsTrue(response.Success);
            Assert.AreEqual(0, response.Errors.Count());
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
            var response = (ApiResponse)result.Value;
            var responseError = response.Errors.First();

            Assert.IsFalse(response.Success);
            Assert.AreEqual(0, response.Warnings.Count());
            Assert.AreEqual(_error.Detail, responseError.Detail);
            Assert.AreEqual(_error.Title, responseError.Title);
            Assert.AreEqual(_error.Status, responseError.Status);
            Assert.AreEqual(_error.Type.OriginalString, responseError.Type.OriginalString);
            Assert.AreEqual(_error.Instance.OriginalString, responseError.Instance.OriginalString);
        }

        [TestMethod]
        public void TestResponseFromErrorList()
        {
            _resultBuilder.WithErrors(new List<ApiError> { _error });
            var result = _resultBuilder.Build();
            var response = (ApiResponse)result.Value;
            var responseError = response.Errors.First();

            Assert.IsFalse(response.Success);
            Assert.AreEqual(0, response.Warnings.Count());
            Assert.AreEqual(_error.Detail, responseError.Detail);
            Assert.AreEqual(_error.Title, responseError.Title);
            Assert.AreEqual(_error.Status, responseError.Status);
            Assert.AreEqual(_error.Type.OriginalString, responseError.Type.OriginalString);
            Assert.AreEqual(_error.Instance.OriginalString, responseError.Instance.OriginalString);
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
    }
}
