using AbacatePay;
using AbacatePay.Models.Coupon;
using Microsoft.AspNetCore.Mvc;

namespace AbacatePayTestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AbacatePayCouponController : ControllerBase
{
    private readonly AbacatePayClient _abacatePayClient;
    private readonly ILogger<AbacatePayCouponController> _logger;

    public AbacatePayCouponController(AbacatePayClient abacatePayClient, ILogger<AbacatePayCouponController> logger)
    {
        _abacatePayClient = abacatePayClient;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo coupon
    /// </summary>
    /// <param name="request">Dados do coupon</param>
    /// <returns>Informações do coupon criado</returns>
    [HttpPost("create")]
    public async Task<ActionResult<CouponResponse>> CreateCoupon([FromBody] CouponRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating coupon: {CouponCode}", request.Data?.Code);

            var response = await _abacatePayClient.CreateCouponAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating coupon");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Lista todos os coupons
    /// </summary>
    /// <returns>Lista de coupons</returns>
    [HttpGet("list")]
    public async Task<ActionResult<List<CouponResponse>>> ListCoupons()
    {
        try
        {
            _logger.LogInformation("Listing all coupons");

            var response = await _abacatePayClient.ListCouponsAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing coupons");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
