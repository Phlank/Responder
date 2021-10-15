using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace Phlank.Responder.Tests.Data
{
    public static class TestData
    {
        public static Warning WarningWithExtensions = new Warning(
            Severity.Critical,
            "TestMessage",
            extensions: new Dictionary<string, object>() {
                { "Code", "TestCode" },
                { "Fields", new List<string>() { "TestField" } },
            });

        public static Warning Warning = new Warning(
            Severity.High,
            "WarningMessage");

        public static ApiError ApiError400 = new ApiError(HttpStatusCode.BadRequest);
    }
}
