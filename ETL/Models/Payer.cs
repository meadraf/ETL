namespace ETL.Models;

public class Payer
{
    public string Name { get; set; } = string.Empty;
    public decimal Payment { get; set; }
    public DateTime Date { get; set; }
    public long AccountNumber { get; set; }
}