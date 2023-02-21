namespace ETL.CLI;

public class CLI
{
    private Action Restart;

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
        Console.WriteLine("Started!");
        
        var dataProcessingManager = new DataProcessingManager();
        Restart += dataProcessingManager.Reset;
        Restart += Start;
        Task.Run(()=>dataProcessingManager.StartService());
    }

    private void Reset()
    {
        Restart.Invoke();
        Console.WriteLine("Restarted!");
    }

    private void Stop()
    {
        
    }

}