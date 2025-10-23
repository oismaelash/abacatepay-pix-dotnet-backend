using AbacatePayTestApi.Models;
using AbacatePay;
using AbacatePay.Models;
using AbacatePay.Models.Billing;
using AbacatePay.Models.PixQrCode;

namespace AbacatePayTestApi.Services;

public class AbacatePayService : IAbacatePayService
{
    private readonly AbacatePayClient _abacatePayClient;
    private readonly ILogger<AbacatePayService> _logger;

    public AbacatePayService(AbacatePayClient abacatePayClient, ILogger<AbacatePayService> logger)
    {
        _abacatePayClient = abacatePayClient;
        _logger = logger;
    }

    public async Task<PaymentResponse> CreatePaymentAsync(PaymentRequest request)
    {
        try
        {
            _logger.LogInformation("Creating billing for amount: {Amount} {Currency}", request.Amount, request.Currency);

            // Convert our PaymentRequest to AbacatePay BillingRequest
            var billingRequest = new BillingRequest
            {
                Frequency = BillingFrequency.ONE_TIME,
                Methods = new List<BillingPaymentMethod> { BillingPaymentMethod.PIX },
                Products = new List<BillingProduct>
                {
                    new BillingProduct
                    {
                        ExternalId = Guid.NewGuid().ToString(),
                        Name = request.Description,
                        Description = request.Description,
                        Quantity = 1,
                        Price = (int)(request.Amount * 100) // Convert to cents
                    }
                },
                ReturnUrl = request.ReturnUrl ?? "https://example.com/return",
                CompletionUrl = "https://example.com/completion",
                Customer = !string.IsNullOrEmpty(request.CustomerId) ? null : new BillingCustomerData
                {
                    Name = request.CustomerName ?? "Customer",
                    Email = request.CustomerEmail ?? "customer@example.com",
                    Cellphone = "11999999999",
                    TaxId = "12345678901"
                },
                CustomerId = request.CustomerId
            };

            var billingResponse = await _abacatePayClient.CreateBillingAsync(billingRequest);

            var response = new PaymentResponse
            {
                IsSuccess = true,
                PaymentId = billingResponse.Id,
                Status = billingResponse.Status,
                Amount = billingResponse.Amount / 100.0m, // Convert from cents
                Currency = "BRL",
                CreatedAt = billingResponse.CreatedAt,
                PaymentUrl = billingResponse.Url,
                Metadata = request.Metadata
            };

            _logger.LogInformation("Billing created successfully with ID: {PaymentId}", response.PaymentId);
            return response;
        }
        catch (AbacatePayException ex)
        {
            _logger.LogError(ex, "AbacatePay error creating billing");
            return new PaymentResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating billing");
            return new PaymentResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<PaymentResponse> CreatePixPaymentAsync(PixPaymentRequest request)
    {
        try
        {
            _logger.LogInformation("Creating PIX QRCode for amount: {Amount} {Currency}", request.Amount, request.Currency);

            // Convert our PixPaymentRequest to AbacatePay PixQrCodeRequest
            var pixQrCodeRequest = new PixQrCodeRequest
            {
                Amount = (int)(request.Amount * 100), // Convert to cents
                ExpiresIn = request.ExpirationMinutes * 60, // Convert minutes to seconds
                Description = request.Description,
                Customer = new PixQrCodeCustomer
                {
                    Name = request.PayerName ?? "Customer",
                    Email = request.CustomerEmail ?? "customer@example.com",
                    Cellphone = "11999999999",
                    TaxId = "12345678901"
                },
                Metadata = new Dictionary<string, object>
                {
                    ["pixKey"] = request.PixKey ?? "Not provided",
                    ["originalRequest"] = "PixPaymentRequest"
                }
            };

            var pixQrCodeResponse = await _abacatePayClient.CreatePixQrCodeAsync(pixQrCodeRequest);

            var response = new PaymentResponse
            {
                IsSuccess = true,
                PaymentId = pixQrCodeResponse.Id,
                Status = pixQrCodeResponse.Status,
                Amount = pixQrCodeResponse.Amount / 100.0m, // Convert from cents
                Currency = "BRL",
                CreatedAt = pixQrCodeResponse.CreatedAt,
                PaymentUrl = pixQrCodeResponse.BrCode ?? "", // PIX QR Code
                Metadata = new Dictionary<string, object>
                {
                    ["pixKey"] = request.PixKey ?? "Not provided",
                    ["expirationMinutes"] = request.ExpirationMinutes,
                    ["payerName"] = request.PayerName ?? "Not provided",
                    ["brCode"] = pixQrCodeResponse.BrCode ?? "",
                    ["brCodeBase64"] = pixQrCodeResponse.BrCodeBase64 ?? "",
                    ["expiresAt"] = pixQrCodeResponse.ExpiresAt?.ToString() ?? ""
                }
            };

            _logger.LogInformation("PIX QRCode created successfully with ID: {PaymentId}", response.PaymentId);
            return response;
        }
        catch (AbacatePayException ex)
        {
            _logger.LogError(ex, "AbacatePay error creating PIX QRCode");
            return new PaymentResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating PIX QRCode");
            return new PaymentResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<PaymentResponse> GetPaymentStatusAsync(string paymentId)
    {
        try
        {
            _logger.LogInformation("Getting billing status for ID: {PaymentId}", paymentId);

            // Try to get billing first, then try PIX QRCode if billing fails
            try
            {
                var billingResponse = await _abacatePayClient.GetBillingAsync(paymentId);
                
                var response = new PaymentResponse
                {
                    IsSuccess = true,
                    PaymentId = billingResponse.Id,
                    Status = billingResponse.Status,
                    Amount = billingResponse.Amount / 100.0m, // Convert from cents
                    Currency = "BRL",
                    CreatedAt = billingResponse.CreatedAt,
                    PaymentUrl = billingResponse.Url
                };

                _logger.LogInformation("Billing status retrieved: {Status} for ID: {PaymentId}", response.Status, paymentId);
                return response;
            }
            catch (AbacatePayException)
            {
                // If billing not found, try PIX QRCode
                var pixStatusResponse = await _abacatePayClient.CheckPixQrCodeStatusAsync(paymentId);
                
                var response = new PaymentResponse
                {
                    IsSuccess = true,
                    PaymentId = paymentId,
                    Status = pixStatusResponse.Status,
                    Currency = "BRL",
                    CreatedAt = DateTime.UtcNow // PIX QRCode doesn't return creation date in status
                };

                _logger.LogInformation("PIX QRCode status retrieved: {Status} for ID: {PaymentId}", response.Status, paymentId);
                return response;
            }
        }
        catch (AbacatePayException ex)
        {
            _logger.LogError(ex, "AbacatePay error getting payment status for ID: {PaymentId}", paymentId);
            return new PaymentResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment status for ID: {PaymentId}", paymentId);
            return new PaymentResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<PaymentResponse> CancelPaymentAsync(string paymentId)
    {
        try
        {
            _logger.LogInformation("Cancelling payment with ID: {PaymentId}", paymentId);

            // Note: The AbacatePay SDK doesn't have a direct cancel method
            // This is a placeholder implementation. In a real scenario, you would need to:
            // 1. Check if the payment/billing can be cancelled based on its status
            // 2. Use the appropriate API endpoint to cancel the payment
            // 3. Handle different payment types (billing vs PIX QRCode)

            // For now, we'll just check the status and return a simulated cancellation
            var statusResponse = await GetPaymentStatusAsync(paymentId);
            
            if (!statusResponse.IsSuccess)
            {
                return statusResponse;
            }

            // Check if payment can be cancelled (only pending payments can be cancelled)
            if (statusResponse.Status != "pending" && statusResponse.Status != "created")
            {
                return new PaymentResponse
                {
                    IsSuccess = false,
                    ErrorMessage = $"Payment cannot be cancelled. Current status: {statusResponse.Status}"
                };
            }

            // In a real implementation, you would call the cancel API here
            var response = new PaymentResponse
            {
                IsSuccess = true,
                PaymentId = paymentId,
                Status = "cancelled",
                CreatedAt = DateTime.UtcNow,
                ErrorMessage = "Payment cancellation simulated - implement actual cancellation logic"
            };

            _logger.LogInformation("Payment cancellation simulated for ID: {PaymentId}", paymentId);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling payment for ID: {PaymentId}", paymentId);
            return new PaymentResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }
}
