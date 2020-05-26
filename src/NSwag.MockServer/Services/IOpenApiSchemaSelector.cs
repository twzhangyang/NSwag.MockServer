using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace NSwag.MockServer.Services
{
    public interface IOpenApiSchemaSelector
    {
        Tuple<int, OpenApiObject> Select(OpenApiOperation operation);
    }

    public class OpenApiSchemaSelector : IOpenApiSchemaSelector
    {
        private readonly ILogger<OpenApiSchemaSelector> _logger;
        private readonly IOpenApiSchemaTransformer _schemaTransformer;

        public OpenApiSchemaSelector(ILogger<OpenApiSchemaSelector> logger, IOpenApiSchemaTransformer schemaTransformer)
        {
            _logger = logger;
            _schemaTransformer = schemaTransformer;
        }

        public Tuple<int, OpenApiObject> Select(OpenApiOperation operation)
        {
            var responses = operation.Responses;

            //Don't have response status code
            if (responses.Count == 0)
            {
                _logger.LogError($"response is empty from open api operation");
                throw new OpenApiSchemaSelectFailedException();
            }

            //Don't have response schema
            if (responses.First().Value.Content.Count == 0)
            {
                return new Tuple<int, OpenApiObject>(int.Parse(responses.First().Key), new OpenApiObject());
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

            IOpenApiAny example;
            if (response.Schema.Example == null)
            {
                var schema = response.Schema.Properties;
                // Generate response by schema
                example = _schemaTransformer.Transform(schema);
            }
            else
            {
                example = response.Schema.Example;
            }

            return new Tuple<int, OpenApiObject>(int.Parse(responses.First().Key), (OpenApiObject) example);
        }
    }

    public class OpenApiSchemaSelectFailedException : MockServerException
    {
        public OpenApiSchemaSelectFailedException() : base("Can not select api schema")
        {
        }
    }
}