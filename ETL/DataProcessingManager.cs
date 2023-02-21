using ETL.Configuration;
using ETL.DataProcessors;

namespace ETL;

public class DataProcessingManager
{
    private DateTime _date = DateTime.Today;
    private int _todayProcessedFiles;
    private readonly FileSystemWatcher _watcher = new FileSystemWatcher(FilesConfiguration.InputFolderPath);
        
    public async Task StartService(CancellationTokenSource cancellationTokenSource)
    {
        await ProcessExistingFilesAsync(cancellationTokenSource);
    }

    private async Task ProcessExistingFilesAsync(CancellationTokenSource cancellationTokenSource)
    {
        var txtFiles = Directory.GetFiles(FilesConfiguration.InputFolderPath, "*.txt");
        var csvFiles = Directory.GetFiles(FilesConfiguration.InputFolderPath, "*.csv");
        foreach (var file in txtFiles)
        {
            await ProcessFileAsync(file, new TxtReader());
        }

        foreach (var file in csvFiles)
        {
            await ProcessFileAsync(file, new CsvReader());
        }

        _watcher.EnableRaisingEvents = true;
        _watcher.Created += OnFileCreatedAsync;
    }
    
    private async Task ProcessFileAsync(string path, IDataReader dataReader)
    {
        var datalist = await dataReader.ReadDataAsync(path);
        File.Delete(path);

        if (datalist.Any())
            _todayProcessedFiles++;
        else 
            return;
        
        var dataSaver = new DataSaver();
        dataSaver.SaveData(DataComposer.Compose(datalist), _todayProcessedFiles);
    }
    
    private async void OnFileCreatedAsync(object sender, FileSystemEventArgs e)
    {
        if (Path.GetExtension(e.FullPath) == ".txt")
              await ProcessFileAsync(e.FullPath, new TxtReader());
        
        if (Path.GetExtension(e.FullPath) == ".csv")
              await ProcessFileAsync(e.FullPath, new CsvReader());
    }
}