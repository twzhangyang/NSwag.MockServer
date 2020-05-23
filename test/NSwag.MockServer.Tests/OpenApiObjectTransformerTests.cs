using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSwag.MockServer.Services;
using NSwag.MockServer.Tests.FakedServices;
using Xunit;

namespace NSwag.MockServer.Tests
{
    public class OpenApiObjectTransformerTests : TestBase
    {
        [Fact]
        public void ShouldTransformToObject()
        {
            //Arrange
            var document = Reader.Read();
            var schemaSelector = ServiceProvider.GetService<IOpenApiSchemaSelector>();
            var operationMatcher = ServiceProvider.GetService<IOpenApiOperationMatcher>();
            var transformer = ServiceProvider.GetService<IOpenApiObjectTransformer>();

            var httpContext = new FakedHttpContext().ModifyRequest(r => { r.Method = "Post"; });

            //Act
            var operation = operationMatcher.MatchByRequestAction(document.Paths["/pet"], httpContext);
            var schema = schemaSelector.Select(operation);
            var o = transformer.Transform(schema);

            //Assert
            var str = JsonConvert.SerializeObject(o);
            str.Should().Be("{\"Id\":1234,\"Name\":\"Add pet\",\"Category\":{\"Id\":1111,\"Name\":\"dog\"}}");
        }
    }
}