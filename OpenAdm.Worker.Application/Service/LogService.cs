namespace OpenAdm.Worker.Application.Service;

public static class LogService
{
    public static void Info(string message)
    {
        Write(message, ConsoleColor.White);
    }

    public static void Success(string message)
    {
        Write(message, ConsoleColor.Green);
    }

    public static void Warning(string message)
    {
        Write(message, ConsoleColor.Yellow);
    }

    public static void Error(string message)
    {
        Write(message, ConsoleColor.Red);
    }

    public static void Debug(string message)
    {
        Write(message, ConsoleColor.Cyan);
    }

    private static void Write(string message, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
        
        Console.ForegroundColor = originalColor;
    }
}