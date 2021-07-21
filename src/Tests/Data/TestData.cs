using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

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
            Code = "TestCode",
            Fields = new List<string> { "TestField" },
            Message = "TestMessage"
        };

        public static ApiBehaviorOptions ApiBehaviorOptions = new ApiBehaviorOptions
        {
            SuppressMapClientErrors = true,
            SuppressConsumesConstraintForFormFileParameters = true,
            SuppressInferBindingSourcesForParameters = true,
            InvalidModelStateResponseFactory = e => null,
        };
    }
}
