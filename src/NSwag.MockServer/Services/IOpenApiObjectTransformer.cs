using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.OpenApi.Any;
using SharpYaml.Tokens;

namespace NSwag.MockServer.Services
{
    public interface IOpenApiObjectTransformer
    {
        object Transform(OpenApiObject openApiObject);
    }

    public class OpenApiObjectTransformer : IOpenApiObjectTransformer
    {
        public object Transform(OpenApiObject openApiObject)
        {
            dynamic root = new ExpandoObject();

            foreach (var (key, value) in openApiObject)
            {
                TransformRec(key, value, root);
            }

            return root;
        }

        private void TransformRec(string key, IOpenApiAny property, dynamic node)
        {
            if (property.AnyType == AnyType.Array)
            {
                var list = new List<dynamic>();
                foreach (var item in ((OpenApiArray) property))
                {
                    dynamic p = new ExpandoObject();

                    if (item.AnyType == AnyType.Primitive)
                    {
                        p = ((dynamic)item).Value;
                    }
                    else
                    {
                        foreach (var (s, value) in (OpenApiObject) item)
                        {
                            TransformRec(s, value, p);
                        }
                    }

                    list.Add(p);
                }
                ((IDictionary<string, object>) node).Add(key, list);
            }
            else if (property.AnyType == AnyType.Object)
            {
                dynamic p = new ExpandoObject();
                foreach (var (s, value) in (OpenApiObject) property)
                {
                    TransformRec(s, value, p);
                }

                ((IDictionary<string, object>) node).Add(key, p);
            }
            else if (property.AnyType == AnyType.Null)
            {
                ((IDictionary<string, object>) node).Add(key, null);
            }
            else
            {
                var value = ((dynamic) property).Value;
                ((IDictionary<string, object>) node).Add(key, value);
            }
        }
    }
}