using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace Phlank.ApiModeling.Tests.Data
{
    public static class TestData
    {
        public static ApiWarning Warning = new ApiWarning
        {
            Code = "TestCode",
            Fields = new List<string> { "TestField" },
            Message = "TestMessage",
            Severity = Severity.Critical
        };

        public static ApiError Error = new ApiError
        {
            Status = HttpStatusCode.BadRequest,
            Detail = "TestDetail",
            Title = "TestTitle",
            Instance = new Uri("about:blank"),
            Type = new Uri("about:blank")
        };

        public static ApiError Error2 = new ApiError
        {
            Status = HttpStatusCode.BadRequest,
            Detail = "TestDetail2",
            Title = "TestTitle2",
            Instance = new Uri("about:blank"),
            Type = new Uri("about:blank")
        };

        public static ApiError Error3 = new ApiError
        {
            Status = HttpStatusCode.BadRequest,
            Detail = "TestDetail3",
            Title = "TestTitle3",
            Instance = new Uri("about:blank"),
            Type = new Uri("about:blank")
        };

        public static string Content = "TestContent";

        public static ApiBehaviorOptions ApiBehaviorOptions = new ApiBehaviorOptions
        {
            SuppressMapClientErrors = true,
            SuppressConsumesConstraintForFormFileParameters = true,
            SuppressInferBindingSourcesForParameters = true,
            InvalidModelStateResponseFactory = e => null,
        };
    }
}
