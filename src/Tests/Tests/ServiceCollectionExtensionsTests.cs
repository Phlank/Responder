using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phlank.ApiModeling.Extensions;
using Phlank.ApiModeling.Tests.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phlank.ApiModeling.Tests.Tests
{
    [TestClass]
    public class ServiceCollectionExtensionsTests
    {
        IServiceCollection _services;

        [TestInitialize]
        public void Initialize()
        {
            _services = new ServiceCollection();
        }

        [TestMethod]
        public void TestConfigureApiResponseBuilderAddsService()
        {
            _services.ConfigureApiResponseBuilder();
            var provider = _services.BuildServiceProvider();

            Assert.IsNotNull(provider.GetService<IApiResponseBuilder>());
        }

        [TestMethod]
        public void TestUseApiMethodForModelStateErrorsCreatesNewOptions()
        {
            _services.ConfigureApiResponseBuilder(options =>
            {
                options.UseApiResponseForModelStateErrors();
            });

            var provider = _services.BuildServiceProvider();
            var apiBehaviorOptions = provider.GetService<IOptions<ApiBehaviorOptions>>().Value;

            Assert.IsNotNull(apiBehaviorOptions);
            Assert.IsNotNull(apiBehaviorOptions.InvalidModelStateResponseFactory);
        }

        [TestMethod]
        public void TestUseApiMethodForModelStateErrorsOnlyReplacesBuilder()
        {
            _services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = e => null;
                options.SuppressMapClientErrors = true;
                options.SuppressModelStateInvalidFilter = true;
                options.AllowInferringBindingSourceForCollectionTypesAsFromQuery = true;
            });

            _services.ConfigureApiResponseBuilder(options =>
            {
                options.UseApiResponseForModelStateErrors();
            });

            var provider = _services.BuildServiceProvider();
            var apiBehaviorOptions = provider.GetService<IOptions<ApiBehaviorOptions>>().Value;

            Assert.IsNotNull(apiBehaviorOptions);
            Assert.IsNotNull(apiBehaviorOptions.InvalidModelStateResponseFactory);
            Assert.IsTrue(apiBehaviorOptions.SuppressMapClientErrors);
            Assert.IsTrue(apiBehaviorOptions.SuppressModelStateInvalidFilter);
            Assert.IsTrue(apiBehaviorOptions.AllowInferringBindingSourceForCollectionTypesAsFromQuery);
        }
    }
}
