using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Validations;
using NSwag.MockServer.Services;

namespace NSwag.MockServer
{
    public class OpenApiDocumentStreamReader
    {
        private readonly IOpenApiDocumentValidator _validator;

        public OpenApiDocumentStreamReader(IOpenApiDocumentValidator validator)
        {
            _validator = validator;
        }
        public OpenApiDocument Read(FileInfo input)
        {
            using Stream stream = input.OpenRead();
            var document = new OpenApiStreamReader(new OpenApiReaderSettings
                {
                    ReferenceResolution = ReferenceResolutionSetting.ResolveLocalReferences,
                    RuleSet = ValidationRuleSet.GetDefaultRuleSet()
                }
            ).Read(stream, out var context);

            if (!_validator.IsValid(document))
            {
                throw new InValidSwaggerException();
            }
                
            return document;
        }
    }

    public class InValidSwaggerException : Exception
    {
        public InValidSwaggerException() : base("Incorrect swagger document")
        {
            
        }
    }
}