using System;
using System.Dynamic;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSwag.MockServer.Services;
using Xunit;

namespace NSwag.MockServer.Tests
{
    public class TestBase
    {
        protected ConventionalFolderSwaggerDocumentSource OpenAPiDocumentReader => ServiceProvider.GetService<ConventionalFolderSwaggerDocumentSource>();
        
        protected IServiceProvider ServiceProvider { get; set; }
        
        public TestBase()
        {
            var services = new ServiceCollection();
            services.AddMockServer();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}