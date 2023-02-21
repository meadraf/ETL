using ETL.Configuration;
using ETL.DataProcessors;
using ETL.Models;

namespace ETL;

public class DataProcessingManager
{
    private DateTime _date = DateTime.Today;
    private MetaLog _metalog = new();
    private readonly FileSystemWatcher _watcher = new(FilesConfiguration.InputFolderPath);
    private Thread? _dateChecker;
    private bool _isStopped = false;

    public async Task StartService()
    {
        _dateChecker = new Thread(CheckDate);
        _dateChecker.Start();
        
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
            _metalog.InvalidFiles.Add(path);
        else
            FillMetaLog(dataReader);
           
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

    private void FillMetaLog(IDataReader dataReader)
    {
        _metalog.ParsedLines += dataReader.ParsedLines;
        _metalog.ParsedFiles += 1;
        _metalog.FoundErrors += dataReader.InvalidLines;
    }
    
    public void Reset()
    {
        _watcher.EnableRaisingEvents = false;
        _watcher.Dispose();
        _isStopped = true;

    }

    private void CheckDate()
    {
        while (!_isStopped)
        {
            Thread.Sleep(1000);
            if (_date != DateTime.Today)
            {
                var logSaver = new LogSaver();
                logSaver.SaveLog(_metalog, _date);
                _date = DateTime.Today;

                _metalog = new MetaLog();
            }
        }
    }
}