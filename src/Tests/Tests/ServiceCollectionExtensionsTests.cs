using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Phlank.ApiModeling.Extensions;
using Phlank.ApiModeling.Tests.Helpers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Phlank.ApiModeling.Tests
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

            Assert.IsNotNull(provider.GetService<IApiResultBuilder>());
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
            });

            _services.ConfigureApiResponseBuilder(options =>
            {
                options.UseApiResponseForModelStateErrors();
            });

            var provider = _services.BuildServiceProvider();
            var apiBehaviorOptions = provider.GetService<IOptions<ApiBehaviorOptions>>().Value;

            Assert.IsNotNull(apiBehaviorOptions);
            Assert.IsNotNull(apiBehaviorOptions.InvalidModelStateResponseFactory(new ActionContext()));
            Assert.IsTrue(apiBehaviorOptions.SuppressMapClientErrors);
            Assert.IsTrue(apiBehaviorOptions.SuppressModelStateInvalidFilter);
        }

        [TestMethod]
        public async Task TestInvalidModelStateResponseFactoryAsync()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
            var client = server.CreateClient();
            var model = new TestModel
            {
                Between1And2 = 3.0
            };
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var result = await client.PostAsync("/api/TestMethod", content);
            var resultContentBody = await result.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ApiError>(resultContentBody);

            Assert.AreEqual("application/problem+json", result.Content.Headers.ContentType.MediaType);
            Assert.IsTrue(error.Extensions.ContainsKey("trace"));
            Assert.IsTrue(error.Extensions.ContainsKey("otherErrors"));
        }
    }
}
