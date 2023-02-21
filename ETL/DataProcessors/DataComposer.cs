using ETL.Models;

namespace ETL.DataProcessors;

public class DataComposer
{
    public static List<OutputData> Compose(IEnumerable<InputData> inputData)
    {
        var outputData = inputData
            .GroupBy(data => data.Address.Split(",", StringSplitOptions.RemoveEmptyEntries).First())
            .Select(g => new OutputData
            {
                City = g.Key,
                Services = g.GroupBy(s => s.Service)
                    .Select(g => new Service
                    {
                        Name = g.Key,
                        Payers = g.Select(data => new Payer
                        {
                            Name = (data.FirstName + " " + data.LastName).Trim(),
                            Payment = data.Payment,
                            Date = data.Date,
                            AccountNumber = data.AccountNumber
                        }).ToList(),
                        Total = g.Sum(r => r.Payment),
                    }).ToList(),
                Total = g.Sum(r => r.Payment)
            }).ToList();

        return outputData;
    }
}