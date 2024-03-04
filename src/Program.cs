﻿public class Program
{
    // <summary>
    // The main entry point for the application.
    // </summary>
    private static void Main()
    {
        Init();
        Loop();
    }

    // <summary>
    // Initializes all controllers.
    // </summary>
    private static void Init()
    {
        CustomerController.GetInstance();
    }

    // <summary>
    // The main loop of the application.
    // </summary>
    private static void Loop()
    {
        while (true)
        {
            try
            {
                string raw = ChatUtil.GetInlineInput("Enter command");
                string[] commandToken = raw.Trim().Split(' ');

                if (commandToken.Length == 0)
                {
                    ChatUtil.PrintHelp();
                    continue;
                }

                string command = commandToken[0].ToLower();
                string[] parameters = commandToken.Skip(1).ToArray();

                switch (command)
                {
                    case "ls":
                        ListAll(parameters);
                        continue;
                    case "add":
                        Add(parameters);
                        continue;
                    case "update":
                        Update(parameters);
                        continue;
                    case "rm":
                        Remove(parameters);
                        continue;
                    case "save":
                        CustomerController.GetInstance().Save();
                        continue;
                    case "exit":
                        Console.WriteLine("Bye!");
                        break;
                    case "help":
                        ChatUtil.PrintHelp();
                        continue;
                    default:
                        Console.WriteLine($"Unknown command \"{command}\"!");
                        goto case "help";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Some Error Occurred:");
                Console.WriteLine(e.Message);
                continue;
            }
            finally
            {
                Console.WriteLine(); // empty line
            }
        }
    }

    // <summary>
    // Lists all models of a given type.
    // If a second parameter is given, it will filter the results.
    // </summary>
    // <param name="token">The command token.</param>
    private static void ListAll(string[] token)
    {

        if (token.Length < 1 || token.Length > 2)
        {
            ChatUtil.PrintLsHelp();
            throw new Exception("Invalid Syntax. Expecting at least one parameter");
        }

        // if (token.Length == 2)
        // {
        //     ListFilter(token);
        //     return;
        // }

        switch (token[0])
        {
            case "customer":
                CustomerController.GetInstance().PrintAll();
                return;
            default:
                ChatUtil.PrintLsHelp();
                throw new Exception($"Unknown model type \"{token[0]}\"!");
        }

    }

    // // <summary>
    // // Lists all assignments by a given person or course.
    // // </summary>
    // private static void ListFilter(string[] token)
    // {
    //     int id = int.Parse(token[1]);
    //     switch (token[0])
    //     {
    //         case "person":
    //             TeilnahmeController.GetInstance().PrintAllForPerson(id);
    //             return;
    //         case "kurs":
    //             TeilnahmeController.GetInstance().PrintAllForKurs(id);
    //             return;
    //         default:
    //             ChatUtil.PrintLsHelp();
    //             throw new Exception($"Unknown model type \"{token[0]}\"!");
    //     }
    // }

    // <summary>
    // Adds a new model of a given type.
    // </summary>
    // <param name="token">The command token.</param>
    private static void Add(string[] token)
    {
        if (token.Length != 1)
        {
            ChatUtil.PrintAddHelp();
            throw new Exception("Invalid Syntax. Expecting exactly one parameter");
        }

        switch (token[0])
        {
            case "customer":
                CustomerController.GetInstance().Add(CreateModelWithUserInput<Customer>(CustomerController.GetInstance().NextFreeId));
                return;
            default:
                ChatUtil.PrintAddHelp();
                throw new Exception($"Unknown model type \"{token[0]}\"!");
        }
    }

    // <summary>
    // Updates a model of a given type.
    // </summary>
    // <param name="token">The command token.</param>
    private static void Update(string[] token)
    {
        if (token.Length != 2)
        {
            ChatUtil.PrintUpdateHelp();
            throw new Exception("Invalid Syntax. Expecting exactly two parameters");
        }

        int id = int.Parse(token[1]);
        switch (token[0])
        {
            case "person":
                CustomerController.GetInstance().Update(id, (Customer customer) => UpdateModelWithUserInput(customer));
                return;
            default:
                ChatUtil.PrintUpdateHelp();
                throw new Exception($"Unknown model type \"{token[0]}\"!");
        }
    }

    // <summary>
    // Creates a new model of a given type and fills it with user input.
    // </summary>
    // <param name="id">The id of the new model.</param>
    // <returns>The new model.</returns>
    private static T CreateModelWithUserInput<T>(int id) where T : AbstractModel<T>, new()
    {
        T model = new T
        {
            ID = id
        };
        return UpdateModelWithUserInput(model);
    }

    // <summary>
    // Updates a model of a given type and fills it with user input.
    // </summary>
    // <param name="model">The model to update.</param>
    // <returns>The updated model.</returns>
    private static T UpdateModelWithUserInput<T>(T model) where T : AbstractModel<T>, new()
    {
        // setup
        Console.WriteLine($"The following details are required for a {model.GetType().Name}:");
        string[] header = model.GetHeader().Split(ChatUtil.FieldDelimiter);
        string[] headerWithoutId = header.Skip(1).ToArray();
        Console.WriteLine(headerWithoutId.Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b));

        List<string> values = model.GetValues().ToList();

        // update values
        for (int i = 1; i < header.Count(); i++) // skip id
        {
            string currentValue = values[i];
            string newValue = ChatUtil.GetInlineInput($"{header[i]} ({currentValue})");
            values[i] = newValue != "" ? newValue : currentValue;
        }

        // confirm input
        if (!ChatUtil.Confirm(
            $"Do you want to save the following {model.GetType().Name}?\n" +
            $"{header.Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b)}\n" +
            $"{values.Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b)}"
            ))
        {
            throw new Exception("Action Aborted!");
        }

        // return final model
        return model.SetValues(values.ToArray());
    }

    // <summary>
    // Removes a model of a given type.
    // </summary>
    // <param name="token">The command token.</param>
    private static void Remove(string[] token)
    {
        if (token.Length != 2)
        {
            ChatUtil.PrintRemoveHelp();
            throw new Exception("Invalid Syntax. Expecting exactly two parameters");
        }

        int id = int.Parse(token[1]);
        switch (token[0])
        {
            case "customer":
                // if (TeilnahmeController.GetInstance().GetAllForPerson(id).Length > 0)
                // {
                //     throw new Exception("Cannot remove a student that is assigned to a course!");
                // }
                // if (KursController.GetInstance().GetByDozentId(id).Length > 0)
                // {
                //     throw new Exception("Cannot remove a dozent that is assigned to a course!");
                // }
                CustomerController.GetInstance().Remove(id);
                return;
            default:
                throw new Exception($"Unknown model type \"{token[0]}\"!");
        }
    }

}
