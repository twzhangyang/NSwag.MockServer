using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSwag.MockServer.Services;
using NSwag.MockServer.Tests.FakedServices;
using Xunit;

namespace NSwag.MockServer.Tests
{
    public class OpenApiSchemaSelectorTests : TestBase
    {
        [Fact]
       public async void ShouldSelectResponseSchema()
       {
           //Arrange
           var document = await OpenAPiDocumentReader.Read();
           var schemaSelector = ServiceProvider.GetService<IOpenApiSchemaSelector>();
           var operationMatcher = ServiceProvider.GetService<IOpenApiOperationMatcher>();
           
           var httpContext = new FakedHttpContext().ModifyRequest(r =>
           {
               r.Method = "Post";
           }); 
           
           //Act
           var operation = operationMatcher.MatchByRequestAction(document.Paths["/pet"], httpContext);
           var schema = schemaSelector.Select(operation);
           
           //Assert
           schema.Should().NotBeNull();
       } 
    }
}