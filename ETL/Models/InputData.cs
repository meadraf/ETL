namespace ETL.Models;

public class InputData
{
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public decimal Payment { get; set; }
    public DateTime Date { get; set; }
    public long AccountNumber { get; set; }
    public string? Service { get; set; } = string.Empty;
}