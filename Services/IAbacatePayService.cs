using AbacatePayTestApi.Models;

namespace AbacatePayTestApi.Services;

public interface IAbacatePayService
{
    Task<PaymentResponse> CreatePaymentAsync(PaymentRequest request);
    Task<PaymentResponse> CreatePixPaymentAsync(PixPaymentRequest request);
    Task<PaymentResponse> GetPaymentStatusAsync(string paymentId);
    Task<PaymentResponse> CancelPaymentAsync(string paymentId);
}
