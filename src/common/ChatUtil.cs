internal abstract class ChatUtil
{
    // <summary>
    // The delimiter used to separate fields in the data files.
    // </summary>
    public static string FieldDelimiter { get; } = "|";

    // <summary>
    // The delimiter used to separate subfields in the data files.
    // </summary>
    public static string SubFieldDelimiter { get; } = ":";

    // <summary>
    // Gather input from the user on the same line as the message.
    // </summary>
    // <param name="message">The message to display.</param>
    // <returns>The trimmed input.</returns>
    public static string GetInlineInput(string message) {
        Console.Write($"{message}: ");
        return (Console.ReadLine() ?? "").Trim();
    }

    // <summary>
    // Gather input from the user on a new line.
    // </summary>
    // <param name="message">The message to display.</param>
    // <returns>The trimmed input.</returns>
    public static string GetInput(string message)
    {
        Console.WriteLine($"{message}: ");
        return (Console.ReadLine() ?? "").Trim();
    }

    // <summary>
    // Gather binary response from the user.
    // </summary>
    // <param name="message">The message to display.</param>
    // <returns>True if the user responded with "y", false otherwise.</returns>
    public static bool Confirm(string message)
    {
        return GetInput(message + " (y/n)").ToLower() == "y";
    }

    // <summary>
    // Print all help message.
    // </summary>
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

    // <summary>
    // Print help message for ls command.
    // </summary>
    public static void PrintLsHelp()
    {
        Console.WriteLine("ls <model> [<filter>]");
        Console.WriteLine("  <model>   - person | kurs");
        Console.WriteLine("  <filter>  - <id>");
    }

    // <summary>
    // Print help message for add command.
    // </summary>
    public static void PrintAddHelp()
    {
        Console.WriteLine("add <model>");
        Console.WriteLine("  <model>   - person | kurs");
    }

    // <summary>
    // Print help message for update command.
    // </summary>
    public static void PrintUpdateHelp()
    {
        Console.WriteLine("update <model> <id>");
        Console.WriteLine("  <model>   - person | kurs");
        Console.WriteLine("  <id>      - <id>");
    }

    // <summary>
    // Print help message for remove command.
    // </summary>
    public static void PrintRemoveHelp()
    {
        Console.WriteLine("rm <model> <id>");
        Console.WriteLine("  <model>   - person | kurs");
        Console.WriteLine("  <id>      - <id>");
    }

    // <summary>
    // Print help message for assign command.
    // </summary>
    public static void PrintAssignHelp()
    {
        Console.WriteLine("assign <personId> <kursId>");
        Console.WriteLine("  <personId>   - <id>");
        Console.WriteLine("  <kursId>     - <id>");
    }

    // <summary>
    // Print help message for dismiss command.
    // </summary>
    public static void PrintDismissHelp()
    {
        Console.WriteLine("dismiss <personId> <kursId>");
        Console.WriteLine("  <personId>   - <id>");
        Console.WriteLine("  <kursId>     - <id>");
    }

    // <summary>
    // Print help message for grade command.
    // </summary>
    public static void PrintGradeHelp()
    {
        Console.WriteLine("grade <personId> <kursId> [<note>]");
        Console.WriteLine("  <personId>   - <id>");
        Console.WriteLine("  <kursId>     - <id>");
        Console.WriteLine("  <note>       - <float>");
    }
}