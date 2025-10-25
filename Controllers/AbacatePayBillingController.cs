using AbacatePay;
using AbacatePay.Models.Billing;
using Microsoft.AspNetCore.Mvc;

namespace AbacatePayTestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AbacatePayBillingController : ControllerBase
{
    private readonly AbacatePayClient _abacatePayClient;
    private readonly ILogger<AbacatePayBillingController> _logger;

    public AbacatePayBillingController(AbacatePayClient abacatePayClient, ILogger<AbacatePayBillingController> logger)
    {
        _abacatePayClient = abacatePayClient;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo billing
    /// </summary>
    /// <param name="request">Dados do billing</param>
    /// <returns>Informações do billing criado</returns>
    [HttpPost("create")]
    public async Task<ActionResult<BillingResponse>> CreateBilling([FromBody] BillingRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating billing with frequency: {Frequency}", request.Frequency);

            var response = await _abacatePayClient.CreateBillingAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating billing");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Obtém detalhes de um billing
    /// </summary>
    /// <param name="id">ID do billing</param>
    /// <returns>Detalhes do billing</returns>
    [HttpGet]
    public async Task<ActionResult<BillingResponse>> GetBilling([FromQuery] string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { error = "Billing ID is required" });
            }

            _logger.LogInformation("Getting billing with ID: {BillingId}", id);

            var response = await _abacatePayClient.GetBillingAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting billing for ID: {BillingId}", id);
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Lista todos os billings
    /// </summary>
    /// <returns>Lista de billings</returns>
    [HttpGet("list")]
    public async Task<ActionResult<List<BillingResponse>>> ListBillings()
    {
        try
        {
            _logger.LogInformation("Listing all billings");

            var response = await _abacatePayClient.ListBillingsAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing billings");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
