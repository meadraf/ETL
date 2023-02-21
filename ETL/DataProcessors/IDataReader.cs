using ETL.Models;

namespace ETL.DataProcessors;

public interface IDataReader
{
    public int InvalidLines { get; }
    public int ParsedLines { get; }
    public Task<List<InputData>> ReadDataAsync(string path);
}