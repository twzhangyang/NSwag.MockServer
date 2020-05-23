using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSwag.MockServer.Services;
using Xunit;

namespace NSwag.MockServer.Tests
{
    public class OpenApiDocumentReaderTests : TestBase
    {
        [Fact]
        public void ShouldReadOpenApiDocumentCorrectly()
        {
            //Arrange
            var reader = ServiceProvider.GetService<ConventionalFolderOpenApiDocumentSource>();
            
            //Act
            var document = reader.Read();
            
            //Assert
            document.Should().NotBeNull();
        }
    }
}