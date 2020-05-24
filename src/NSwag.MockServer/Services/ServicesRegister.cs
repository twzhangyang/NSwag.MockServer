using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace NSwag.MockServer.Services
{
    public static class ServicesRegister
    {
       public static void AddMockServer(this IServiceCollection services)
       {
           services.AddScoped<IOpenApiDocumentValidator, OpenApiDocumentValidator>();
           services.AddScoped<OpenApiDocumentStreamReader>();
           services.AddScoped<OpenApiDocumentStringReader>();
           services.AddScoped<IOpenApiPathItemMatcher, OpenApiPathItemMatcher>();
           services.AddScoped<IOpenApiOperationMatcher, OpenApiOperationMatcher>();
           services.AddScoped<IOpenApiSchemaSelector, OpenApiSchemaSelector>();
           services.AddScoped<IOpenApiObjectTransformer, OpenApiObjectTransformer>();

           services.AddScoped<IOpenApiDocumentSource, ConventionalFolderOpenApiDocumentSource>();
           services.AddScoped<IOpenApiDocumentSource, UrlBasedOpenApiDocumentSource>();
           services.AddScoped<ConventionalFolderOpenApiDocumentSource>();
           services.AddScoped<UrlBasedOpenApiDocumentSource>();

           services.AddHttpClient("mockServer")
               .ConfigurePrimaryHttpMessageHandler(() =>
               {
                   return new HttpClientHandler
                   {
                       ServerCertificateCustomValidationCallback = (a, b, c, d) => true
                   };
               });

       } 
    }
}