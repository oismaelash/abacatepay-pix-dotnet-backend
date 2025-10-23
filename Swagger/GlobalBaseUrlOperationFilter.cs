using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AbacatePayTestApi.Swagger;

public class GlobalBaseUrlOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Adicionar parâmetro global de Base URL para todos os endpoints
        operation.Parameters ??= new List<OpenApiParameter>();
        
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-Base-Url",
            In = ParameterLocation.Header,
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Default = new Microsoft.OpenApi.Any.OpenApiString("https://api.abacatepay.com")
            },
            Description = "AbacatePay Base URL (opcional, usa o valor padrão se não informado)"
        });
    }
}
