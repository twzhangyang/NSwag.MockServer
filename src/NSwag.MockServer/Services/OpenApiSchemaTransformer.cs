using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace NSwag.MockServer.Services
{
    public interface IOpenApiSchemaTransformer
    {
        OpenApiObject Transform(IDictionary<string, OpenApiSchema> properties);
    }

    public class OpenApiSchemaTransformer : IOpenApiSchemaTransformer
    {
        public OpenApiObject Transform(IDictionary<string, OpenApiSchema> properties)
        {
            var root = new OpenApiObject();
            foreach (var (key, property) in properties)
            {
                Transform(key, property, root);
            }

            return root;
        }

        private void Transform(string key, OpenApiSchema schema, OpenApiObject openApiObject)
        {
            var type = schema.Type;
            var format = schema.Format;

            if (type == "object")
            {
                var item = new OpenApiObject();
                foreach (var (itemKey, itemProperty) in schema.Properties)
                {
                    Transform(itemKey, itemProperty, item);
                }

                openApiObject.Add(key, item);
            }

            else if (type == "array")
            {
                if (schema.Items.Type == "object")
                {
                    var item = new OpenApiObject();
                    foreach (var (itemKey, itemProperty) in schema.Items.Properties)
                    {
                        Transform(itemKey, itemProperty, item);
                    }

                    var items = new OpenApiArray {item};
                    openApiObject.Add(key, items);
                }
                else
                {
                    var items = new OpenApiArray();
                    var item = GetOpenApiValue(schema.Items);
                    items.Add(item);
                    openApiObject.Add(key, items);
                }
            }
            else
            {
                var openApiValue = GetOpenApiValue(schema);
                openApiObject.Add(key, openApiValue);
            }
        }

        private IOpenApiAny GetOpenApiValue(OpenApiSchema schema)
        {
            var type = schema.Type;
            var format = schema.Format;

            if (type == "integer" && format == "int32")
            {
                return new OpenApiInteger(1);
            }

            if (type == "integer" && format == "int64")
            {
                return new OpenApiLong(1234);
            }

            if (type == "number" && format == "float")
            {
                return new OpenApiFloat(123.12f);
            }

            if (type == "number" && format == "double")
            {
                return new OpenApiDouble(123.12d);
            }

            if (type == "string" && format == "byte")
            {
                return new OpenApiByte(Convert.FromBase64String("aGVsbG8="));
            }

            if (type == "string" && format == "binary")
            {
                return new OpenApiBinary(Encoding.UTF8.GetBytes("abc"));
            }

            if (type == "string" && format == "date")
            {
                return new OpenApiDate(DateTime.Now.Date);
            }

            if (type == "string" && format == "date-time")
            {
                return new OpenApiDateTime(DateTimeOffset.Now);
            }

            if (type == "string" && format == "password")
            {
                return new OpenApiPassword("password");
            }

            if (type == "string" && schema.Enum.Any())
            {
                return new OpenApiString(((dynamic)schema.Enum.First()).Value);
            }

            if (type == "string")
            {
                return new OpenApiString("hello");
            }
            
            if (type == "boolean")
            {
                return new OpenApiBoolean(true);
            }
            
            return new OpenApiNull();
        }
    }
}