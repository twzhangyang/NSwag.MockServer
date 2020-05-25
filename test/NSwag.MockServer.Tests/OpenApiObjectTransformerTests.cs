using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Newtonsoft.Json;
using NSwag.MockServer.Services;
using NSwag.MockServer.Tests.FakedServices;
using Xunit;

namespace NSwag.MockServer.Tests
{
    public class OpenApiObjectTransformerTests : TestBase
    {
        private readonly IOpenApiSchemaSelector _schemaSelector;
        private readonly IOpenApiOperationMatcher _operationMatcher;
        private readonly IOpenApiObjectTransformer _transformer;

        public OpenApiObjectTransformerTests()
        {
             _schemaSelector = ServiceProvider.GetService<IOpenApiSchemaSelector>();
             _operationMatcher = ServiceProvider.GetService<IOpenApiOperationMatcher>();
             _transformer = ServiceProvider.GetService<IOpenApiObjectTransformer>();
        }
        
        [Fact]
        public async void ShouldTransformToObject()
        {
            //Arrange
            var document = await OpenAPiDocumentReader.Read();
            var httpContext = new FakedHttpContext().ModifyRequest(r => { r.Method = "Post"; });

            //Act
            var operation = _operationMatcher.MatchByRequestAction(document.Paths["/pet"], httpContext);
            var schema = _schemaSelector.Select(operation);
            var o = _transformer.Transform(schema.Item2);

            //Assert
            var str = await JsonConvert.SerializeObjectAsync(o);
            str.Should().Be("{\"Id\":1234,\"Name\":\"Add pet\",\"Category\":{\"Id\":1111,\"Name\":\"dog\"}}");
        }
        
        [Fact]
        public async void WhenApiDidNotHaveResponseTypeShouldOnlyGetStatusCode()
        {
            //Arrange
            var document = await OpenAPiDocumentReader.Read();
            var httpContext = new FakedHttpContext().ModifyRequest(r => { r.Method = "Put"; });

            //Act
            var operation = _operationMatcher.MatchByRequestAction(document.Paths["/pet"], httpContext);
            var schema = _schemaSelector.Select(operation);
            
            //Assert
            schema.Item2.Count.Should().Be(0);
            schema.Item1.Should().Be(200);
        }
    }
}