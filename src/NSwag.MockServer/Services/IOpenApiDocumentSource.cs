using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace NSwag.MockServer.Services
{
    public interface IOpenApiDocumentSource
    {
        bool IsValid { get; }
        
        int Priority { get; }

        Task<OpenApiDocument> Read();
    }

    public class ConventionalFolderOpenApiDocumentSource : IOpenApiDocumentSource
    {
        private readonly OpenApiDocumentStreamReader _reader;

        private readonly string _filePath = $"swagger{Path.DirectorySeparatorChar}swagger.json";

        public bool IsValid => File.Exists(_filePath);
        
        public int Priority => 1;

        public ConventionalFolderOpenApiDocumentSource(OpenApiDocumentStreamReader reader)
        {
            _reader = reader;
        }
        
        public Task<OpenApiDocument> Read()
        {
            var fileInfo = new FileInfo(_filePath);
            return Task.FromResult(_reader.Read(fileInfo));
        }
    }

    public class UrlBasedOpenApiDocumentSource : IOpenApiDocumentSource
    {
        private readonly IConfiguration _configuration;
        private readonly OpenApiDocumentStringReader _reader;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UrlBasedOpenApiDocumentSource> _logger;

        private string urlKey = "swaggerUrl";

        public bool IsValid => !string.IsNullOrEmpty(_configuration[urlKey]);
        
        public int Priority => 2;

        public UrlBasedOpenApiDocumentSource(IConfiguration configuration, OpenApiDocumentStringReader reader,
            IHttpClientFactory httpClientFactory,
            ILogger<UrlBasedOpenApiDocumentSource> logger)
        {
            _configuration = configuration;
            _reader = reader;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        
        public async Task<OpenApiDocument> Read()
        {
            var httpClient = _httpClientFactory.CreateClient("mockServer");

            try
            {
                var httpResponseMessage = await httpClient.GetAsync(_configuration[urlKey]);

                if(httpResponseMessage.IsSuccessStatusCode)
                {
                    var content = await httpResponseMessage.Content.ReadAsStringAsync();
                    return _reader.Read(content);
                }
                _logger.LogError( $"read swagger failed from {_configuration[urlKey]}");
                return null;
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"read swagger failed from {_configuration[urlKey]}");
                return null;
            }
        }
    }
}