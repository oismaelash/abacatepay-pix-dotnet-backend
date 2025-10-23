using AbacatePayTestApi.Models;
using AbacatePayTestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AbacatePayTestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AbacatePayController : ControllerBase
{
    private readonly IAbacatePayService _abacatePayService;
    private readonly ILogger<AbacatePayController> _logger;

    public AbacatePayController(IAbacatePayService abacatePayService, ILogger<AbacatePayController> logger)
    {
        _abacatePayService = abacatePayService;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo pagamento
    /// </summary>
    /// <param name="request">Dados do pagamento</param>
    /// <returns>Informações do pagamento criado</returns>
    [HttpPost("create-payment")]
    public async Task<ActionResult<PaymentResponse>> CreatePayment([FromBody] PaymentRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _abacatePayService.CreatePaymentAsync(request);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Cria um novo pagamento PIX
    /// </summary>
    /// <param name="request">Dados do pagamento PIX</param>
    /// <returns>Informações do pagamento PIX criado</returns>
    [HttpPost("create-pix-payment")]
    public async Task<ActionResult<PaymentResponse>> CreatePixPayment([FromBody] PixPaymentRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _abacatePayService.CreatePixPaymentAsync(request);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating PIX payment");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Obtém o status de um pagamento
    /// </summary>
    /// <param name="paymentId">ID do pagamento</param>
    /// <returns>Status do pagamento</returns>
    [HttpGet("payment/{paymentId}/status")]
    public async Task<ActionResult<PaymentResponse>> GetPaymentStatus(string paymentId)
    {
        try
        {
            if (string.IsNullOrEmpty(paymentId))
            {
                return BadRequest(new { error = "Payment ID is required" });
            }

            var response = await _abacatePayService.GetPaymentStatusAsync(paymentId);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment status for ID: {PaymentId}", paymentId);
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Cancela um pagamento
    /// </summary>
    /// <param name="paymentId">ID do pagamento</param>
    /// <returns>Resultado do cancelamento</returns>
    [HttpPost("payment/{paymentId}/cancel")]
    public async Task<ActionResult<PaymentResponse>> CancelPayment(string paymentId)
    {
        try
        {
            if (string.IsNullOrEmpty(paymentId))
            {
                return BadRequest(new { error = "Payment ID is required" });
            }

            var response = await _abacatePayService.CancelPaymentAsync(paymentId);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling payment for ID: {PaymentId}", paymentId);
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    /// <summary>
    /// Endpoint de teste para verificar se a API está funcionando
    /// </summary>
    /// <returns>Informações da API</returns>
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            service = "AbacatePay Test API",
            version = "1.0.0"
        });
    }
}
