using System.Text.Json;
using ETL.Configuration;
using ETL.Models;

namespace ETL;

public class DataSaver
{
    public void SaveData(List<OutputData> data)
    {
        var folderPath = FilesConfiguration.OutputFolderPath + "/" + DateTime.Now.ToString("MM-dd-yyyy") + "/";
        CreateFolder(folderPath);
        var options = new JsonSerializerOptions {WriteIndented = true};
        using var stream = File.OpenWrite(folderPath + "/output" + GetNumber(folderPath) + ".json");
        JsonSerializer.Serialize(stream, data, typeof(List<OutputData>), options: options);
    }

    private void CreateFolder(string path)
    {
        Directory.CreateDirectory(path);
    }

    private int GetNumber(string path)
    {
        var directoryInfo = new DirectoryInfo(path);
        return directoryInfo.GetFiles("*.json").Length + 1;
    }
}