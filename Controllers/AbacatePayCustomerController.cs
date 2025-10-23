using AbacatePay;
using AbacatePay.Models;
using AbacatePay.Models.Customer;
using Microsoft.AspNetCore.Mvc;

namespace AbacatePayTestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AbacatePayCustomerController : ControllerBase
{
    private readonly AbacatePayClient _abacatePayClient;
    private readonly AbacatePayConfig _defaultConfig;
    private readonly ILogger<AbacatePayCustomerController> _logger;

    public AbacatePayCustomerController(AbacatePayClient abacatePayClient, AbacatePayConfig defaultConfig, ILogger<AbacatePayCustomerController> logger)
    {
        _abacatePayClient = abacatePayClient;
        _defaultConfig = defaultConfig;
        _logger = logger;
    }

    private AbacatePayClient GetAbacatePayClient(string? bearerToken = null, string? baseUrl = null)
    {
        if (string.IsNullOrEmpty(bearerToken))
        {
            throw new UnauthorizedAccessException("API Key é obrigatória. Forneça o Bearer Token no header Authorization.");
        }

        var config = new AbacatePayConfig
        {
            ApiKey = bearerToken,
            BaseUrl = baseUrl ?? _defaultConfig.BaseUrl,
            Sandbox = _defaultConfig.Sandbox,
            TimeoutSeconds = _defaultConfig.TimeoutSeconds
        };

        return new AbacatePayClient(config);
    }

    /// <summary>
    /// Cria um novo customer
    /// </summary>
    /// <param name="request">Dados do customer</param>
    /// <param name="authorization">Bearer Token com API Key do AbacatePay (obrigatório)</param>
    /// <param name="baseUrl">Base URL do AbacatePay (opcional, usa a configuração padrão se não informado)</param>
    /// <returns>Informações do customer criado</returns>
    [HttpPost("create")]
    public async Task<ActionResult<CustomerResponse>> CreateCustomer(
        [FromBody] CustomerRequest request,
        [FromHeader(Name = "Authorization")] string authorization,
        [FromHeader(Name = "X-Base-Url")] string? baseUrl = null)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating customer: {CustomerName}", request.Name);

            // Extrair Bearer Token do header Authorization
            string bearerToken;
            if (string.IsNullOrEmpty(authorization))
            {
                return Unauthorized(new { error = "Header Authorization é obrigatório" });
            }
            
            if (!authorization.StartsWith("Bearer "))
            {
                return Unauthorized(new { error = "Formato inválido. Use: Bearer <api-key>" });
            }
            
            bearerToken = authorization.Substring(7).Trim();
            if (string.IsNullOrEmpty(bearerToken))
            {
                return Unauthorized(new { error = "API Key não pode estar vazia" });
            }

            var client = GetAbacatePayClient(bearerToken, baseUrl);
            var response = await client.CreateCustomerAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Lista todos os customers
    /// </summary>
    /// <param name="authorization">Bearer Token com API Key do AbacatePay (obrigatório)</param>
    /// <param name="baseUrl">Base URL do AbacatePay (opcional, usa a configuração padrão se não informado)</param>
    /// <returns>Lista de customers</returns>
    [HttpGet("list")]
    public async Task<ActionResult<List<CustomerResponse>>> ListCustomers(
        [FromHeader(Name = "Authorization")] string authorization,
        [FromHeader(Name = "X-Base-Url")] string? baseUrl = null)
    {
        try
        {
            _logger.LogInformation("Listing all customers");

            // Extrair Bearer Token do header Authorization
            string bearerToken;
            if (string.IsNullOrEmpty(authorization))
            {
                return Unauthorized(new { error = "Header Authorization é obrigatório" });
            }
            
            if (!authorization.StartsWith("Bearer "))
            {
                return Unauthorized(new { error = "Formato inválido. Use: Bearer <api-key>" });
            }
            
            bearerToken = authorization.Substring(7).Trim();
            if (string.IsNullOrEmpty(bearerToken))
            {
                return Unauthorized(new { error = "API Key não pode estar vazia" });
            }

            var client = GetAbacatePayClient(bearerToken, baseUrl);
            var response = await client.ListCustomersAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing customers");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
