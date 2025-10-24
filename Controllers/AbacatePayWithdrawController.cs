using AbacatePay;
using Microsoft.AspNetCore.Mvc;
using AbacatePay.Models.Withdraw;
namespace AbacatePayTestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AbacatePayWithdrawController : ControllerBase
{
    private readonly ILogger<AbacatePayWithdrawController> _logger;
    private readonly AbacatePayClient _abacatePayClient;
    public AbacatePayWithdrawController(AbacatePayClient abacatePayClient, ILogger<AbacatePayWithdrawController> logger)
    {
        _abacatePayClient = abacatePayClient;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo withdraw
    /// </summary>
    /// <param name="request">Dados do withdraw</param>
    /// <returns>Informações do withdraw criado (resposta direta do SDK)</returns>
    [HttpPost("create")]
    public async Task<ActionResult<WithdrawData>> CreateWithdraw([FromBody] WithdrawRequest request)
    {
        try
        {
            _logger.LogInformation("Creating withdraw");

            // request.Pix.Type

            var response = await _abacatePayClient.CreateWithdrawAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating withdraw");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Obtém detalhes de um withdraw por ID
    /// </summary>
    /// <param name="externalId">ID do withdraw</param>
    /// <returns>Detalhes do withdraw (resposta direta do SDK)</returns>
    [HttpGet("get")]
    public async Task<ActionResult> GetWithdraw([FromQuery] string externalId)
    {  
        try
        {
            if (string.IsNullOrWhiteSpace(externalId))
            {
                return BadRequest(new { error = "Withdraw ID é obrigatório" });
            }

            _logger.LogInformation("Getting withdraw details for ID: {WithdrawId}", externalId);

            var response = await _abacatePayClient.GetWithdrawAsync(externalId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting withdraw with ID: {WithdrawId}", externalId);
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Lista todos os withdraws
    /// </summary>
    /// <returns>Lista de withdraws (resposta direta do SDK)</returns>
    [HttpGet("list")]
    public async Task<ActionResult> ListWithdraws()
    {
        try
        {
            _logger.LogInformation("Listing all withdraws");

            var response = await _abacatePayClient.ListWithdrawsAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing withdraws");
            return StatusCode(500, new { error = ex.Message });
        }
    }

}
