using AbacatePay;
using AbacatePay.Models.PixQrCode;
using Microsoft.AspNetCore.Mvc;

namespace AbacatePayTestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AbacatePayPixController : ControllerBase
{
    private readonly AbacatePayClient _abacatePayClient;
    private readonly ILogger<AbacatePayPixController> _logger;

    public AbacatePayPixController(AbacatePayClient abacatePayClient, ILogger<AbacatePayPixController> logger)
    {
        _abacatePayClient = abacatePayClient;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo PIX QRCode
    /// </summary>
    /// <param name="request">Dados do PIX QRCode</param>
    /// <returns>Informações do PIX QRCode criado</returns>
    [HttpPost("qrcode/create")]
    public async Task<ActionResult<PixQrCodeResponse>> CreatePixQrCode([FromBody] PixQrCodeRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating PIX QRCode with amount: {Amount}", request.Amount);

            var response = await _abacatePayClient.CreatePixQrCodeAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating PIX QRCode");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Verifica o status de um PIX QRCode
    /// </summary>
    /// <param name="id">ID do PIX QRCode</param>
    /// <returns>Status do PIX QRCode</returns>
    [HttpGet("qrcode/check-status")]
    public async Task<ActionResult<PixQrCodeStatusResponse>> CheckPixQrCodeStatus([FromQuery] string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { error = "PIX QRCode ID is required" });
            }

            _logger.LogInformation("Checking PIX QRCode status for ID: {PixQrCodeId}", id);

            var response = await _abacatePayClient.CheckPixQrCodeStatusAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking PIX QRCode status for ID: {PixQrCodeId}", id);
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Simula um pagamento PIX (apenas em modo dev)
    /// </summary>
    /// <param name="pixQrCodeId">ID do PIX QRCode</param>
    /// <param name="request">Dados da simulação</param>
    /// <returns>Resultado da simulação</returns>
    [HttpPost("qrcode/simulate")]
    public async Task<ActionResult<PixQrCodeData>> SimulatePixPayment([FromQuery] string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { error = "PIX QRCode ID is required" });
            }

            _logger.LogInformation("Simulating PIX payment for QRCode ID: {PixQrCodeId}", id);

            var response = await _abacatePayClient.SimulatePixQrCodePaymentAsync(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error simulating PIX payment for ID: {PixQrCodeId}", id);
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
