using ETL.CLI;
using ETL.Configuration;

var filesConfiguration = new FilesConfiguration("config.json");

var cli = new CLI();
cli.Run();