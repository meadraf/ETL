namespace ETL.Models;

public class OutputData
{
    public string City { get; set; } = string.Empty;
    public List<Service> Services { get; set; } = new();
    public decimal Total { get; set; }
}