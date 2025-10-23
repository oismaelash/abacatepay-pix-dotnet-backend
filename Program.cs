using AbacatePayTestApi.Services;
using AbacatePayTestApi.Swagger;
using AbacatePay;
using AbacatePay.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "AbacatePay Test API",
        Version = "v1",
        Description = "API para testes do AbacatePay SDK"
    });

    // Configurar Bearer Token para API Key
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "API Key",
        Description = "AbacatePay API Key como Bearer Token"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Adicionar parâmetro global para Base URL
    c.OperationFilter<GlobalBaseUrlOperationFilter>();
});

// Configure AbacatePay SDK
var abacatePayConfig = builder.Configuration.GetSection("AbacatePay").Get<AbacatePayConfig>() ?? new AbacatePayConfig();
builder.Services.AddSingleton(abacatePayConfig);
builder.Services.AddScoped<AbacatePayClient>();

// Register custom AbacatePay service wrapper
builder.Services.AddScoped<IAbacatePayService, AbacatePayService>();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
