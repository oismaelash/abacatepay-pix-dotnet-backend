namespace AbacatePayTestApi.Models;

public class PaymentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "BRL";
    public string Description { get; set; } = string.Empty;
    public string? CustomerId { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerName { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
    public string? ReturnUrl { get; set; }
    public string? CancelUrl { get; set; }
}
