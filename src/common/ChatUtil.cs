public abstract class ChatUtil {
    public static string FieldDelimiter { get; } = "|";
    public static string SubFieldDelimiter { get; } = ":";

    public static bool Confirm(string message)
    {
        Console.WriteLine($"{message} (y/n)");
        string? input = Console.ReadLine();
        return (input ?? "n").Trim().ToLower() == "y" ;
    }
}