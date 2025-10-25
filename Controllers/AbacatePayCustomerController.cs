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
    public async Task<ActionResult<CustomerResponse>> CreateCustomer([FromBody] CustomerRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating customer: {CustomerName}", request.Name);
            
            var response = await _abacatePayClient.CreateCustomerAsync(request);
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
    public async Task<ActionResult<List<CustomerResponse>>> ListCustomers()
    {
        try
        {
            _logger.LogInformation("Listing all customers");

            var response = await _abacatePayClient.ListCustomersAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing customers");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
