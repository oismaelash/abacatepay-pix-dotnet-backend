namespace AbacatePayTestApi.Models;

public class PaymentResponse
{
    public bool IsSuccess { get; set; }
    public string? PaymentId { get; set; }
    public string? Status { get; set; }
    public string? ErrorMessage { get; set; }
    public string? PaymentUrl { get; set; }
    public decimal Amount { get; set; }
    public string? Currency { get; set; }
    public DateTime CreatedAt { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
}
