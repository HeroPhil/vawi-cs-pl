﻿using System.Collections;
using System.Data.Common;
using System.Xml.Linq;

class Program
{
    static void Main()
    {

        init();
        Loop();
    }

    static void init()
    {
        PersonController.GetInstance();
        KursController.GetInstance();
    }

    static void Loop()
    {
        while (true)
        {
            Console.WriteLine("Enter a command:");
            string? command = Console.ReadLine();

            if (command == null || command.Trim() == "")
            {
                ShowHelp();
                continue;
            }

            string[] commandToken = command.Trim().Split(' ');

            if (commandToken.Length == 0)
            {
                ShowHelp();
                continue;
            }

            switch (commandToken[0].ToLower())
            {
                case "ls":
                    ListAll(commandToken.Skip(1).ToArray());
                    continue;
                case "add":
                    Add(commandToken.Skip(1).ToArray());
                    continue;
                case "save":
                    PersonController.GetInstance().Save();
                    KursController.GetInstance().Save();
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

    static void ListAll(string[] token)
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
                    PersonController.GetInstance().PrintAll();
                    return;
                case "kurs":
                    KursController.GetInstance().PrintAll();
                    return;
            }
        }

    }

    static void Add(string[] token)
    {
        if (token.Length < 1)
        {
            ShowHelp();
            return;
        }

        try
        {
            switch (token[0])
            {
                case "person":
                    PersonController.GetInstance().Add(CreateBeanWithUserInput<Person>(PersonController.GetInstance().NextFreeId()));
                    return;
                case "kurs":
                    KursController.GetInstance().Add(CreateBeanWithUserInput<Kurs>(KursController.GetInstance().NextFreeId()));
                    return;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    static T CreateBeanWithUserInput<T>(int id) where T : DataBean<T>, new()
    {
        // setup
        T bean = new T();
        Console.WriteLine($"The following details are required for a new {bean.GetType().Name}:");
        Console.WriteLine(bean.GetHeader());
        List<string> values = new List<string>
        {
            id.ToString()
        };

        // get input
        values.AddRange(bean.GetHeader().Split(AbstractController<T>.FieldDelimiter).Skip(1).Select((field) =>
        {
            Console.Write($"{field}: ");
            return Console.ReadLine();
        })!);

        // confirm input
        Console.WriteLine($"Do you want to save the following {bean.GetType().Name}?");
        Console.WriteLine(bean.GetHeader());
        Console.WriteLine(values.Aggregate((a, b) => a + AbstractController<T>.FieldDelimiter + b));
        Console.WriteLine("y/n");
        string? confirm = Console.ReadLine();
        if (confirm == null || confirm.Trim().ToLower() != "y")
        {
            throw new Exception("Adding Aborted!");
        }

        // return final bean
        return bean.SetValues(values.ToArray());
    }
}
