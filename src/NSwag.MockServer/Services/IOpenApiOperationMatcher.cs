using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace NSwag.MockServer.Services
{
    public interface IOpenApiOperationMatcher
    {
        OpenApiOperation MatchByRequestAction(OpenApiPathItem pathItem, HttpContext httpContext);
    }

    public class OpenApiOperationMatcher : IOpenApiOperationMatcher
    {
        private readonly ILogger<OpenApiOperationMatcher> _logger;

        public OpenApiOperationMatcher(ILogger<OpenApiOperationMatcher> logger)
        {
            _logger = logger;
        }
        
        public OpenApiOperation MatchByRequestAction(OpenApiPathItem pathItem, HttpContext httpContext)
        {
            var result = pathItem.Operations
                .TryGetValue(TranslateRequestMethod(httpContext.Request.Method), out var operation);

            if (!result)
            {
                _logger.LogError($"Can not match Open Api Operation by {httpContext.Request.Method}");
                throw new OpenApiOperationMatchFailedException();
            }

            return operation;
        }

        private OperationType TranslateRequestMethod(string method)
        {
            var memberInfo = typeof(OperationType)
                .GetMembers()
                .First(m => m.Name.Equals(method, StringComparison.InvariantCultureIgnoreCase));

            var type = (OperationType)Enum.Parse(typeof(OperationType), memberInfo.Name);
            return type;
        }
        
        public class OpenApiOperationMatchFailedException: MockServerException
        {
            public OpenApiOperationMatchFailedException() : base("Can not find Api operation")
            {
            }
        }
    }
}