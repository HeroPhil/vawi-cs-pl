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

    public static void PrintHelp()
    {
        Console.WriteLine("Available commands:");
        PrintLsHelp();
        PrintAddHelp();
        PrintUpdateHelp();
        PrintRemoveHelp();
        PrintAssignHelp();
        PrintDismissHelp();
        PrintGradeHelp();
        Console.WriteLine("save");
        Console.WriteLine("help");
        Console.WriteLine("exit");
    }

    public static void PrintLsHelp()
    {
        Console.WriteLine("ls <model> [<filter>]");
        Console.WriteLine("  <model>   - person | kurs");
        Console.WriteLine("  <filter>  - <id>");
    }

    public static void PrintAddHelp()
    {
        Console.WriteLine("add <model>");
        Console.WriteLine("  <model>   - person | kurs");
    }

    public static void PrintUpdateHelp()
    {
        Console.WriteLine("update <model> <id>");
        Console.WriteLine("  <model>   - person | kurs");
        Console.WriteLine("  <id>      - <id>");
    }

    public static void PrintRemoveHelp()
    {
        Console.WriteLine("rm <model> <id>");
        Console.WriteLine("  <model>   - person | kurs");
        Console.WriteLine("  <id>      - <id>");
    }

    public static void PrintAssignHelp()
    {
        Console.WriteLine("assign <personId> <kursId>");
        Console.WriteLine("  <personId>   - <id>");
        Console.WriteLine("  <kursId>     - <id>");
    }

    public static void PrintDismissHelp()
    {
        Console.WriteLine("dismiss <personId> <kursId>");
        Console.WriteLine("  <personId>   - <id>");
        Console.WriteLine("  <kursId>     - <id>");
    }

    public static void PrintGradeHelp()
    {
        Console.WriteLine("grade <personId> <kursId> [<note>]");
        Console.WriteLine("  <personId>   - <id>");
        Console.WriteLine("  <kursId>     - <id>");
        Console.WriteLine("  <note>       - <float>");
    }
}