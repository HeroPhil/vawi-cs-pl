public abstract class ChatUtil
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
    // Gather trinary response from the user.
    // </summary>
    // <param name="message">The message to display.</param>
    // <returns>True if the user responded with "y", false if the user responded with "n", null otherwise.</returns>
    public static bool? ConfirmOrCancel(string message)
    {
        string input = GetInput(message + " (y/n/c)").ToLower();
        if (input == "y") return true;
        if (input == "n") return false;
        return null;
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
        PrintRentHelp();
        PrintReturnHelp();
        Console.WriteLine("save");
        Console.WriteLine("help");
        Console.WriteLine("exit");
    }

    // <summary>
    // Print help message for ls command.
    // </summary>
    public static void PrintLsHelp()
    {
        Console.WriteLine("ls <model> [<id>]");
        Console.WriteLine("  <model>   - customer | category | boat | rental");
        Console.WriteLine("  <id>      - <id>");
    }

    // <summary>
    // Print help message for add command.
    // </summary>
    public static void PrintAddHelp()
    {
        Console.WriteLine("add <model>");
        Console.WriteLine("  <model>   - customer | category | boat");
    }

    // <summary>
    // Print help message for update command.
    // </summary>
    public static void PrintUpdateHelp()
    {
        Console.WriteLine("update <model> <id>");
        Console.WriteLine("  <model>   - customer | category | boat | rental");
        Console.WriteLine("  <id>      - <id>");
    }

    // <summary>
    // Print help message for remove command.
    // </summary>
    public static void PrintRemoveHelp()
    {
        Console.WriteLine("rm <model> <id>");
        Console.WriteLine("  <model>   - customer | category | boat | rental");
        Console.WriteLine("  <id>      - <id>");
    }

    // <summary>
    // Print help message for rent command.
    // </summary>
    public static void PrintRentHelp()
    {
        Console.WriteLine("rent");
    }

    // <summary>
    // Print help message for return command.
    // </summary>
    public static void PrintReturnHelp()
    {
        Console.WriteLine("return [<id>]");
        Console.WriteLine("  <id>      - <id>");
    }
}