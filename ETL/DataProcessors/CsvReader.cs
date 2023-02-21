using System.Globalization;
using System.Text.RegularExpressions;
using ETL.Models;

namespace ETL.DataProcessors;

public class CsvReader : IDataReader
{
    public int InvalidLines { get; private set; }
    public int ParsedLines { get; private set; }

    public async Task<List<InputData>> ReadDataAsync(string path)
    {
        const string regexPattern = @"^([\w\s]+),\s([\w\s]+),\s“(.*)”,\s([\d\.]+),\s([\d-]+),\s([\d]+),\s([\w\s]+)$";
        var regex = new Regex(regexPattern);
        var datalist = new List<InputData>();

        using var streamReader = new StreamReader(path);
        await streamReader.ReadLineAsync();
        
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
                    FirstName = match.Groups[1].Value,
                    LastName = match.Groups[2].Value,
                    Address = match.Groups[3].Value,
                    Payment = decimal.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture),
                    Date = DateTime.ParseExact(match.Groups[5].Value, "yyyy-dd-MM", CultureInfo.InvariantCulture),
                    AccountNumber = long.Parse(match.Groups[6].Value),
                    Service = match.Groups[7].Value
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