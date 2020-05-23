using System.Linq;
using Microsoft.OpenApi.Models;

namespace NSwag.MockServer.Services
{
    public interface IOpenApiDocumentValidator
    {
        bool IsValid(OpenApiDocument document);
    }
    
    public class OpenApiDocumentValidator : IOpenApiDocumentValidator
    {
        public bool IsValid(OpenApiDocument document)
        {
            if(!document.Paths.Any())
            {
                return false;
            }

            if (!document.Components.Schemas.Any())
            {
                return false;
            }

            return true;
        }
    }
}