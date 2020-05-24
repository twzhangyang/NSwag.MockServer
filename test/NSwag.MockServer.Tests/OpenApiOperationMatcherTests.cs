using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSwag.MockServer.Services;
using NSwag.MockServer.Tests.FakedServices;
using Xunit;

namespace NSwag.MockServer.Tests.swagger
{
    public class OpenApiOperationMatcherTests : TestBase
    {
        [Fact]
       public async void ShouldGetOpenApiOperationByRequestAction()
       {
           //Arrange
           var document = await OpenAPiDocumentReader.Read();
           var operationMatcher = ServiceProvider.GetService<IOpenApiOperationMatcher>();
           var httpContext = new FakedHttpContext().ModifyRequest(r =>
           {
               r.Method = "Post";
           }); 
           
           //Act
           var operation = operationMatcher.MatchByRequestAction(document.Paths["/pet"], httpContext);
           
           //Assert
           operation.Should().NotBeNull();
       } 
    }
}