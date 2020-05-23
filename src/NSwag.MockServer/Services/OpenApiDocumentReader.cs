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
    public class OpenApiDocumentReader
    {
        private readonly IOpenApiDocumentValidator _validator;

        public OpenApiDocumentReader(IOpenApiDocumentValidator validator)
        {
            _validator = validator;
        }
        public OpenApiDocument Read()
        {
            var input = new FileInfo("swagger/swagger.json");
            using (Stream stream = input.OpenRead())
            {
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
    }

    public class InValidSwaggerException : Exception
    {
        public InValidSwaggerException() : base("Incorrect swagger document")
        {
            
        }
    }
}