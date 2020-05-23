using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSwag.MockServer.Services;
using NSwag.MockServer.Tests.FakedServices;
using Xunit;

namespace NSwag.MockServer.Tests
{
    public class OpenApiPathItemMatcherTests : TestBase
    {
        [Fact]
       public void ShouldGetOpenApiPathItemByUrl()
       {
           //Arrange
           var document = Reader.Read();
           var urlMatcher = ServiceProvider.GetService<IOpenApiPathItemMatcher>();
           var httpContext = new FakedHttpContext().ModifyRequest(r =>
           {
               r.Path = new PathString("/pet");
           }); 
           
           //Act
           var pathItem = urlMatcher.MatchByRequestUrl(document, httpContext);

           //Assert
           pathItem.Should().NotBeNull();
       }
    }
}