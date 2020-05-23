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

            foreach (var property in openApiObject)
            {
                TransformRec(property.Key, property.Value, root);
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
                        foreach (var leafItem in (OpenApiObject) item)
                        {
                            TransformRec(leafItem.Key, leafItem.Value, p);
                        }
                    }

                    list.Add(p);
                }
            }
            else if (property.AnyType == AnyType.Object)
            {
                dynamic p = new ExpandoObject();
                foreach (var item in (OpenApiObject) property)
                {
                    TransformRec(item.Key, item.Value, p);
                }

                ((IDictionary<String, Object>) node).Add(key, p);
            }
            else
            {
                var value = ((dynamic) property).Value;
                ((IDictionary<String, Object>) node).Add(key, value);
            }
        }
    }
}