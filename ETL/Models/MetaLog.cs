namespace ETL.Models;

public class MetaLog
{
    public int ParsedFiles { get; set; }
    public int ParsedLines { get; set; }
    public int FountErrors { get; set; }
    public List<string> InvalidFiles { get; set; } = new();
}