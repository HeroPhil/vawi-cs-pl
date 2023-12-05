using System;

class Program
{
    static void Main()
    {

        PersonController personController = new PersonController();
        personController.PrintAll();
        Person p = new Person(PersonTypEnum.Student, "Max", "Mustermann", "Matura", new DateTime(1990, 1, 1), new Adresse("Musterstrasse", "1", "1234", "Musterort", "Musterland"));
        personController.Add(p);
        personController.PrintAll();


        while (true)
        {
            Console.WriteLine("Enter a command:");
            string command = Console.ReadLine();

            // Process the command here
            // ...

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
}
