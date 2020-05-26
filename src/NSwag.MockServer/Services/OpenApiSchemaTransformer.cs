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
        private readonly RandomValueGenerator _valueGenerator;

        public OpenApiSchemaTransformer(RandomValueGenerator valueGenerator)
        {
            _valueGenerator = valueGenerator;
        }
        
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
                return new OpenApiInteger(_valueGenerator.GenerateInt32());
            }

            if (type == "integer" && format == "int64")
            {
                return new OpenApiLong(_valueGenerator.GenerateInt64());
            }

            if (type == "number" && format == "float")
            {
                return new OpenApiFloat(_valueGenerator.GenerateSingle());
            }

            if (type == "number" && format == "double")
            {
                return new OpenApiDouble(_valueGenerator.GenerateDouble());
            }

            if (type == "string" && format == "byte")
            {
                return new OpenApiByte(_valueGenerator.GenerateByte());
            }

            if (type == "string" && format == "binary")
            {
                return new OpenApiBinary(Encoding.UTF8.GetBytes(_valueGenerator.GenerateString()));
            }

            if (type == "string" && format == "date")
            {
                return new OpenApiDate(_valueGenerator.GenerateDateTime());
            }

            if (type == "string" && format == "date-time")
            {
                return new OpenApiDateTime(new DateTimeOffset(_valueGenerator.GenerateDateTime()));
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
                return new OpenApiString(_valueGenerator.GenerateString());
            }
            
            if (type == "boolean")
            {
                return new OpenApiBoolean(_valueGenerator.GenerateBoolean());
            }
            
            return new OpenApiNull();
        }
    }
}