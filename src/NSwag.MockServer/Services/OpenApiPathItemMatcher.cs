using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

namespace NSwag.MockServer.Services
{
    public interface IOpenApiPathItemMatcher
    {
        OpenApiPathItem MatchByRequestUrl(OpenApiDocument document, HttpContext httpContext);
    }

    public class OpenApiPathItemMatcher : IOpenApiPathItemMatcher
    {
        public OpenApiPathItem MatchByRequestUrl(OpenApiDocument document, HttpContext httpContext)
        {
            var requestUrl = httpContext.Request.Path;
            var requestSections = requestUrl.Value.Split('/').ToList();

            var path = document.Paths.Keys.ToList()
                .Select(p => new {Key = p, Sections = p.Split('/')})
                .Where(s=>s.Sections.Length ==  requestSections.Count)
                .FirstOrDefault(x=>Match(requestSections, x.Sections.ToList()));

            return path == null ? null : document.Paths[path.Key];
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
}