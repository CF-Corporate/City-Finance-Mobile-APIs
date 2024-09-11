using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace GniApi.Helper
{
    public class DefaultValueSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties != null)
            {
                foreach (var property in schema.Properties.Values)
                {
                    // If the property has a default value, set it in the Swagger documentation
                    if (property.Default != null)
                    {
                        property.Example = property.Default;
                    }
                }
            }
        }
    }
}
