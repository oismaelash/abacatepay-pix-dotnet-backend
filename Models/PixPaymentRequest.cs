namespace AbacatePayTestApi.Models;

public class PixPaymentRequest : PaymentRequest
{
    public string? PixKey { get; set; }
    public int ExpirationMinutes { get; set; } = 30;
    public string? PayerName { get; set; }
    public string? PayerDocument { get; set; }
}
