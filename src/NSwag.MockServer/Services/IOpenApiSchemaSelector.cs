using System.Linq;
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
        public OpenApiObject Select(OpenApiOperation operation)
        {

            var responses = operation.Responses;

            if (responses.Count == 0)
            {
                return null;
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
}