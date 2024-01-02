using System.Dynamic;

public abstract class ChatUtil
{
    public static string FieldDelimiter { get; } = "|";
    public static string SubFieldDelimiter { get; } = ":";

    public static string GetInlineInput(string message) {
        Console.Write($"{message}: ");
        return (Console.ReadLine() ?? "").Trim();
    }

    public static string GetInput(string message)
    {
        Console.WriteLine($"{message}: ");
        return (Console.ReadLine() ?? "").Trim();
    }

    public static bool Confirm(string message)
    {
        return GetInput(message + " (y/n)").ToLower() == "y";
    }
}