using ETL.Configuration;

namespace ETL;

public class DataProcessingManager
{
    public async Task StartService(CancellationTokenSource cancellationTokenSource)
    {
        await ProcessExistingFiles(cancellationTokenSource);
    }

    private async Task ProcessExistingFiles(CancellationTokenSource cancellationTokenSource)
    {
        var txtFiles = Directory.GetFiles(FilesConfiguration.InputFolderPath, "*.txt");
        var csvFiles = Directory.GetFiles(FilesConfiguration.InputFolderPath, "*.csv");
        foreach (var file in txtFiles)
        {
            await ProcessFile(file);
        }

        foreach (var file in csvFiles)
        {
            await ProcessFile(file);
        }
    }
    
    private async Task ProcessFile(string path)
    {
         
    }

}