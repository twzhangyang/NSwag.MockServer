using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace NSwag.MockServer.Services
{
    public interface IOpenApiSchemaSelector
    {
        OpenApiObject Select(OpenApiOperation operation);
    }

    public class OpenApiSchemaSelector : IOpenApiSchemaSelector
    {
        private readonly ILogger<OpenApiSchemaSelector> _logger;

        public OpenApiSchemaSelector(ILogger<OpenApiSchemaSelector> logger)
        {
            _logger = logger;
        }
        
        public OpenApiObject Select(OpenApiOperation operation)
        {
            var responses = operation.Responses;

            if (responses.Count == 0)
            {
                _logger.LogError($"response is empty from open api operation");
                throw new OpenApiSchemaSelectFailedException();
            }

            OpenApiMediaType response;
            if (responses.First().Value.Content.ContainsKey("application/json"))
            {
                response = responses.First().Value.Content["application/json"];
            }
            else
            {
                response = responses.First().Value.Content.First().Value;
            }

            var example = response.Schema.Example;
            
            return (OpenApiObject)example;
        }
    }
    
    public class OpenApiSchemaSelectFailedException: MockServerException
    {
        public OpenApiSchemaSelectFailedException() : base("Can not select api schema")
        {
        }
    }

}