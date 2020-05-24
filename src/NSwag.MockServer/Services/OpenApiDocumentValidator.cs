using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace NSwag.MockServer.Services
{
    public interface IOpenApiDocumentValidator
    {
        bool IsValid(OpenApiDocument document);
    }
    
    public class OpenApiDocumentValidator : IOpenApiDocumentValidator
    {
        private readonly ILogger<OpenApiDocumentValidator> _logger;

        public OpenApiDocumentValidator(ILogger<OpenApiDocumentValidator> logger)
        {
            _logger = logger;
        }
        public bool IsValid(OpenApiDocument document)
        {
            if(!document.Paths.Any())
            {
                _logger.LogError("Open api document is invalid because paths is empty");
                return false;
            }

            if (!document.Components.Schemas.Any())
            {
                _logger.LogError("Open api document is invalid because schemas is empty");
                return false;
            }

            return true;
        }
    }
}