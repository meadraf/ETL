using ETL.Configuration;
using ETL.Models;

namespace ETL;

public class LogSaver
{
    public void SaveLog(MetaLog log, DateTime date)
    {
        var folderPath = FilesConfiguration.OutputFolderPath + "/" + date.ToString("MM-dd-yyyy") + "/";
        if (Directory.Exists(folderPath))
        {
            File.WriteAllText(folderPath + "/meta.log", ComposeLog(log));
        }
    }

    private string ComposeLog(MetaLog log)
    {
        var invalidFiles = log.InvalidFiles.Any()
            ? log.InvalidFiles.Aggregate(string.Empty, (current, word) => current + ", " + word)
            : string.Empty;

        return $"parsed_files: {log.ParsedFiles} \n" +
               $"parsed_lines: {log.ParsedLines} \n" +
               $"found_errors: {log.FoundErrors} \n" +
               $"invalid_files: {invalidFiles}";
    }
}