namespace DownloadOptimize.Other;

public static class Logger
{
    public static void WriteToLog(string msg, string logType)
    {
        if (!File.Exists("log.txt"))
        {
            File.Create("log.txt").Close();
        }
        
        File.AppendAllText("log.txt", $"{DateTime.Now} - [{logType}]: {msg}");
    }
    
    public static void Log(string message)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("[");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("LOG");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("]: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
        
        WriteToLog(message, "LOG");
    }
    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("[");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("ERR");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("]: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
        
        WriteToLog(message, "ERROR");
    }
    public static void INFO(string message)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("[");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("INF");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("]: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
        
        WriteToLog(message, "INFO");
    }
}