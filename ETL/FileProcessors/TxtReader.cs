using System.Globalization;
using System.Text.RegularExpressions;
using ETL.Models;

namespace ETL.FileProcessors;

public class TxtReader : IDataReader
{
    public int InvalidLines { get; private set; }
    public int ParsedLines { get; private set; }
    
    public async Task<List<InputData>> ReadData(string path)
    {
        const string regexPattern = @"^([\w\s]+),\s([\w\s]+),\s“(.*)”,\s([\d\.]+),\s([\d-]+),\s([\d]+),\s([\w\s]+)$";
        var regex = new Regex(regexPattern);
        var datalist = new List<InputData>();

        using var streamReader = new StreamReader(path);
        while (!streamReader.EndOfStream)
        {
            var line = await streamReader.ReadLineAsync();
            if (line == null)
            {
                InvalidLines++;
                continue;
            }
            
            var match = regex.Match(line);

            if (match.Success)
            {
                var inputData = new InputData
                {
                    Payment = decimal.Parse(match.Groups[3].Value),
                    Date = DateTime.ParseExact(match.Groups[4].Value, "yyyy-dd-MM", CultureInfo.InvariantCulture),
                    AccountNumber = long.Parse(match.Groups[5].Value),
                    FirstName = match.Groups[0].Value,
                    LastName = match.Groups[1].Value,
                    Address = match.Groups[2].Value,
                    Service = match.Groups[6].Value
                };
                datalist.Add(inputData);
                ParsedLines++;
            }
            else
                InvalidLines++;
        }

        return datalist;
    }
}