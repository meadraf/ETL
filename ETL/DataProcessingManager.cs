using ETL.Configuration;
using ETL.DataProcessors;

namespace ETL;

public class DataProcessingManager
{
    private DateTime _date = DateTime.Today;
    private readonly FileSystemWatcher _watcher = new FileSystemWatcher(FilesConfiguration.InputFolderPath);

    public async Task StartService()
    {
        await ProcessExistingFilesAsync();

        _watcher.EnableRaisingEvents = true;
        _watcher.Created += OnFileCreatedAsync;
    }

    private async Task ProcessExistingFilesAsync()
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
    }

    private async Task ProcessFileAsync(string path, IDataReader dataReader)
    {
        var datalist = await dataReader.ReadDataAsync(path);
        File.Delete(path);

        if (!datalist.Any())
            return;

        var dataSaver = new DataSaver();
        dataSaver.SaveData(DataComposer.Compose(datalist));
    }

    private void OnFileCreatedAsync(object sender, FileSystemEventArgs e)
    {
        if (Path.GetExtension(e.FullPath) == ".txt")
            ProcessFileAsync(e.FullPath, new TxtReader());

        else if (Path.GetExtension(e.FullPath) == ".csv")
            ProcessFileAsync(e.FullPath, new CsvReader());
    }

    public void Reset()
    {
        _watcher.EnableRaisingEvents = false;
        _watcher.Dispose();
    }
}