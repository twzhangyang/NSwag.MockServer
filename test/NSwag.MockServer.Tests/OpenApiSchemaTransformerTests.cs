using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Newtonsoft.Json;
using NSwag.MockServer.Services;
using NSwag.MockServer.Tests.FakedServices;
using Xunit;

namespace NSwag.MockServer.Tests
{
    public class OpenApiSchemaTransformerTests: TestBase
    {
        [Fact]
        public async void ShouldTransformToObject()
        {
            //Arrange
            var operationMatcher = ServiceProvider.GetService<IOpenApiOperationMatcher>();
            var transformer = ServiceProvider.GetService<IOpenApiSchemaTransformer>();
            var pathItemMatcher = ServiceProvider.GetService<IOpenApiPathItemMatcher>();
            var document = await OpenAPiDocumentReader.Read();

            var httpContext = new FakedHttpContext().ModifyRequest(r =>
            {
                r.Method = "Get";
                r.Path = "/1";
            });

            //Act
            var pathItem = pathItemMatcher.MatchByRequestUrl(document, httpContext);
            var operation = operationMatcher.MatchByRequestAction(pathItem, httpContext);
            var response = operation.Responses.First().Value.Content.First().Value;
            var schema = response.Schema.Properties;

            //Assert
            var o = transformer.Transform(schema);
            o.Keys.Count.Should().Be(18);
        }

    }
}