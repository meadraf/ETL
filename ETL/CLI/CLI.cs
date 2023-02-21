using System.Diagnostics;
using System.Security;

namespace ETL.CLI;

public class CLI
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public void Run()
    {
        Console.WriteLine("Commands: \nstart\nreset\nstop\n");
        
        var isStopped = false;
        while (isStopped == false)
        {
            var input = Console.ReadLine();

            switch (input)
            {
                case "start":
                    Start();
                    break;
                
                case "reset":
                    Reset();
                    break;
                
                case "stop":
                    Stop();
                    isStopped = true;
                    break;
                
                default:
                    Console.WriteLine("Invalid command name");
                    break;
            }
        }
    }

    private void Start()
    {
        var dataProcessingManager = new DataProcessingManager();
        Task.Run(()=>dataProcessingManager.StartService(_cancellationTokenSource));
    }

    private void Reset()
    {
        _cancellationTokenSource.Cancel();
        //
    }

    private void Stop()
    {
        _cancellationTokenSource.Cancel();
    }

}