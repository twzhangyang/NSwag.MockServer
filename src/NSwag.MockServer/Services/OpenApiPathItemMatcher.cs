using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace NSwag.MockServer.Services
{
    public interface IOpenApiPathItemMatcher
    {
        OpenApiPathItem MatchByRequestUrl(OpenApiDocument document, HttpContext httpContext);
    }

    public class OpenApiPathItemMatcher : IOpenApiPathItemMatcher
    {
        private readonly ILogger<OpenApiPathItemMatcher> _logger;

        public OpenApiPathItemMatcher(ILogger<OpenApiPathItemMatcher> logger)
        {
            _logger = logger;
        }
        
        public OpenApiPathItem MatchByRequestUrl(OpenApiDocument document, HttpContext httpContext)
        {
            var requestUrl = httpContext.Request.Path;
            var requestSections = requestUrl.Value.Split('/').ToList();

            var path = document.Paths.Keys.ToList()
                .Select(p => new {Key = p, Sections = p.Split('/')})
                .Where(s=>s.Sections.Length ==  requestSections.Count)
                .FirstOrDefault(x=>Match(requestSections, x.Sections.ToList()));

            if (path == null)
            {
                _logger.LogError($"Can not find open api path item by request url:{requestUrl}");
                throw new OpenApiPathItemMatchFailedException();
            }
            
            return document.Paths[path.Key];
        }

        private bool Match(List<string> sectionsA, List<string> sectionsB)
        {
            for (int i = 0; i < sectionsA.Count; i++)
            {
                var sectionA = sectionsA[i];
                var sectionB = sectionsB[i];

                if (sectionB.StartsWith("{") && sectionB.EndsWith("}"))
                {
                }

                if (!sectionA.Equals(sectionB, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class OpenApiPathItemMatchFailedException : MockServerException
    {
        public OpenApiPathItemMatchFailedException() : base("Can not find Api path item")
        {
        }
    }
}