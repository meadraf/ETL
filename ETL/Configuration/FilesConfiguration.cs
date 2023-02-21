using System.Text.Json;

namespace ETL.Configuration;

public class FilesConfiguration
{
    private readonly string? _configPath;
    public static string? InputFolderPath { get; private set; } = string.Empty;
    public static string? OutputFolderPath { get; private set; } = string.Empty;

    public FilesConfiguration(string configPath)
    {
        Directory.SetCurrentDirectory("../../../");
        ExamineConfigPath(configPath);
        _configPath = configPath;
        DeserializeConfig();
    }

    private void ExamineConfigPath(string configPath)
    {
        if (!File.Exists(configPath))
        {
            throw new Exception($"Config path is invalid. \"{configPath}\" file not found");
        }
    }

    private void DeserializeConfig()
    {
        var json = File.ReadAllText(_configPath);
        var path = JsonSerializer.Deserialize<FolderPath>(json);

        if (path.InputFolder is null || path.OutputFolder is null)
        {
            throw new Exception("Config file has invalid content.");
        }

        InputFolderPath = path.InputFolder;
        OutputFolderPath = path.OutputFolder;

        Directory.CreateDirectory(InputFolderPath);
        Directory.CreateDirectory(OutputFolderPath);
    }
}