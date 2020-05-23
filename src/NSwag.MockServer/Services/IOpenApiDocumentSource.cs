using System;
using System.IO;
using Microsoft.OpenApi.Models;

namespace NSwag.MockServer.Services
{
    public interface IOpenApiDocumentSource
    {
        bool IsValid { get; }
        
        int Priority { get; }

        OpenApiDocument Read();
    }

    public class ConventionalFolderOpenApiDocumentSource : IOpenApiDocumentSource
    {
        private readonly OpenApiDocumentStreamReader _reader;

        private readonly string _filePath = $"swagger{Path.DirectorySeparatorChar}swagger.json";

        public bool IsValid => System.IO.File.Exists(_filePath);
        
        public int Priority => 1;

        public ConventionalFolderOpenApiDocumentSource(OpenApiDocumentStreamReader reader)
        {
            _reader = reader;
        }
        
        public OpenApiDocument Read()
        {
            var fileInfo = new FileInfo(_filePath);
            return _reader.Read(fileInfo);
        }
    }
}