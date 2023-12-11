﻿class Program
{
    static PersonController personController;
    static void Main()
    {

        init();
        Loop();
    }

    static void init()
    {
        personController = new PersonController();
    }

    static void Loop()
    {
        while (true)
        {
            Console.WriteLine("Enter a command:");
            string command = Console.ReadLine();

            if (command == null || command.Trim() == "")
            {
                ShowHelp();
                continue;
            }

            string[] commandToken = command.Split(' ');

            if (commandToken.Length == 0)
            {
                ShowHelp();
                continue;
            }

            switch (commandToken[0])
            {
                case "ls":
                    listAll(commandToken.Skip(1).ToArray());
                    continue;
            }



            if (command == "exit")
            {
                Console.WriteLine("Bye!");
                break; // Exit the loop if the command is "exit"
            }

            ShowHelp();
        }
    }

    static void ShowHelp()
    {
        Console.WriteLine("Help!");
    }

    static void listAll(string[] token)
    {

        if (token.Length < 1)
        {
            ShowHelp();
            return;
        }

        foreach (var item in token)
        {
            switch (item)
            {
                case "person":
                    personController.PrintAll();
                    return;
            }
        }

    }
}
