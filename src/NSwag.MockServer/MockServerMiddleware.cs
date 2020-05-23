using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using NSwag.MockServer.Services;

namespace NSwag.MockServer
{
    public class MockServerMiddleware
    {
        private readonly RequestDelegate _next;
        private static OpenApiDocument _documentCache;

        public MockServerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IEnumerable<IOpenApiDocumentSource> sources,
            IOpenApiPathItemMatcher pathItemMatcher,
            IOpenApiOperationMatcher operationMatcher,
            IOpenApiSchemaSelector schemaSelector,
            IOpenApiObjectTransformer transformer)
        {
            _documentCache ??= sources
                .OrderBy(x => x.Priority)
                .FirstOrDefault(x => x.IsValid)
                ?.Read();

            if (_documentCache == null)
            {
                NotFound(context, "Can not read swagger json");
                return;
            }

            var o = new object();
            try
            {
                var pathItem = pathItemMatcher.MatchByRequestUrl(_documentCache, context);
                var operation = operationMatcher.MatchByRequestAction(pathItem, context);
                var schema = schemaSelector.Select(operation);
                o = transformer.Transform(schema);
            }
            catch (MockServerException)
            {
                NotFound(context, "Can not get example from swagger according to request");
                return;
            }

            var json = JsonSerializer.Serialize(o, new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            });

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }

        private void NotFound(HttpContext context, string message)
        {
            context.Response.StatusCode = (int) HttpStatusCode.NotFound;
            context.Response.WriteAsync(message);
        }
    }
}