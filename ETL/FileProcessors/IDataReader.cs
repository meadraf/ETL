using ETL.Models;

namespace ETL.FileProcessors;

public interface IDataReader
{
    public int InvalidLines { get; }
    public int ParsedLines { get; }
    public Task<List<InputData>> ReadData(string path);
}