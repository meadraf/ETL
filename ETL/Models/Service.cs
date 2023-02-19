namespace ETL.Models;

public class Service
{
    public string Name { get; set; } = string.Empty;
    public List<Payer> Payers { get; set; } = new();
    public decimal Total { get; set; }
}