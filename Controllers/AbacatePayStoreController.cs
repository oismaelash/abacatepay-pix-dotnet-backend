using AbacatePay;
using AbacatePay.Models.Store;
using Microsoft.AspNetCore.Mvc;

namespace AbacatePayTestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AbacatePayStoreController : ControllerBase
{
    private readonly AbacatePayClient _abacatePayClient;
    private readonly ILogger<AbacatePayStoreController> _logger;

    public AbacatePayStoreController(AbacatePayClient abacatePayClient, ILogger<AbacatePayStoreController> logger)
    {
        _abacatePayClient = abacatePayClient;
        _logger = logger;
    }

    /// <summary>
    /// Obtém informações da store
    /// </summary>
    /// <returns>Informações da store</returns>
    [HttpGet]
    public async Task<ActionResult<StoreResponse>> GetStore()
    {
        try
        {
            _logger.LogInformation("Getting store information");

            var response = await _abacatePayClient.GetStoreAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting store information");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
