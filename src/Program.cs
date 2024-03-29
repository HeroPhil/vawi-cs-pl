﻿using System.ComponentModel;

public class Program
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
        BoatCategoryController.GetInstance();
        BoatController.GetInstance();
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
                    case "rent":
                        Rent(parameters);
                        continue;
                    case "return":
                        Return(parameters);
                        continue;
                    case "save":
                        CustomerController.GetInstance().Save();
                        BoatCategoryController.GetInstance().Save();
                        BoatController.GetInstance().Save();
                        continue;
                    case "exit":
                        Console.WriteLine("Bye!");
                        break;
                    case "":
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

        if (token.Length == 2)
        {
            ListFilter(token);
            return;
        }

        switch (token[0])
        {
            case "customer":
                CustomerController.GetInstance().PrintAll();
                return;
            case "category":
                BoatCategoryController.GetInstance().PrintAll();
                return;
            case "boat":
                BoatController.GetInstance().PrintAll();
                return;
            case "rental":
                RentalController.GetInstance().PrintAll();
                return;
            default:
                ChatUtil.PrintLsHelp();
                throw new Exception($"Unknown model type \"{token[0]}\"!");
        }

    }

    // <summary>
    // Lists all assignments by a given person or course.
    // </summary>
    private static void ListFilter(string[] token)
    {
        int id = int.Parse(token[1]);
        switch (token[0])
        {
            case "category":
                BoatController.GetInstance().PrintBoatsForCategory(id);
                return;
            case "boat":
                RentalController.GetInstance().PrintRentalsForBoat(id);
                return;
            case "customer":
                RentalController.GetInstance().PrintRentalsForCustomer(id);
                return;
            default:
                ChatUtil.PrintLsHelp();
                throw new Exception($"Unknown model type \"{token[0]}\"!");
        }
    }

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
            case "category":
                BoatCategoryController.GetInstance().Add(CreateModelWithUserInput<BoatCategory>(BoatCategoryController.GetInstance().NextFreeId));
                return;
            case "boat":
                BoatController.GetInstance().Add(CreateModelWithUserInput<Boat>(BoatController.GetInstance().NextFreeId));
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
            case "customer":
                CustomerController.GetInstance().Update(id, (Customer customer) => UpdateModelWithUserInput(customer));
                return;
            case "category":
                BoatCategoryController.GetInstance().Update(id, (BoatCategory category) => UpdateModelWithUserInput(category));
                return;
            case "boat":
                BoatController.GetInstance().Update(id, (Boat boat) => UpdateModelWithUserInput(boat));
                return;
            case "rental":
                if (!ChatUtil.Confirm("WARNING: It is not recommended to update rentals! All protections will be bypassed.\nConsider to remove or return this rental and create a new one.\nDo you want to continue anyway?")) {
                    throw new Exception("Update Aborted!");
                }
                RentalController.GetInstance().Update(id, (Rental rental) => UpdateModelWithUserInput(rental));
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

        bool? confirmed;
        do
        {
            // update values
            for (int i = 1; i < header.Count(); i++) // skip id
            {
                string currentValue = values[i];
                string newValue = ChatUtil.GetInlineInput($"{header[i]} ({currentValue})");
                values[i] = newValue != "" ? newValue : currentValue;
            }
            model.SetValues(values.ToArray());

            // confirm input
            confirmed = ChatUtil.ConfirmOrCancel(
                $"Do you want to save the following {model.GetType().Name}?\n" +
                $"{model.GetDetailedHeader()}\n" +
                $"{model.GetDetailedValues().Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b)}");

            if (confirmed == null)
            {
                throw new Exception("Action Aborted!");
            }

        } while (!confirmed.Value); // repeat until confirmed or canceled

        // return final model
        return model;
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
                if (RentalController.GetInstance().GetAllForCustomer(id).Length > 0)
                {
                    throw new Exception("Cannot remove a customer which still has rentals!");
                }
                CustomerController.GetInstance().Remove(id);
                return;
            case "category":
                if (BoatController.GetInstance().GetByCategoryID(id).Length > 0)
                {
                    throw new Exception("Cannot remove a category which still has boats assigned!");
                }
                BoatCategoryController.GetInstance().Remove(id);
                return;
            case "boat":
                BoatController.GetInstance().Remove(id);
                return;
            case "rental":
                Rental rental = RentalController.GetInstance().GetByID(id);
                if (rental.StartDate < DateOnly.FromDateTime(DateTime.Now) && !rental.returned)
                {
                    throw new Exception("Cannot remove a rental that has already started! Return it first!");
                }
                RentalController.GetInstance().Remove(id);
                return;
            default:
                throw new Exception($"Unknown model type \"{token[0]}\"!");
        }
    }

    // <summary>
    // Rents a boat to a customer.
    // </summary>
    // <param name="token">The command token.</param>
    private static void Rent(string[] token)
    {
        if (token.Length != 0)
        {
            ChatUtil.PrintRentHelp();
            throw new Exception("Invalid Syntax. Rent does not take any parameter");
        }

        Rental request = CreateModelWithUserInput<Rental>(RentalController.GetInstance().NextFreeId);
        while (!RentalController.ValidateRental(request))
        {
            if (!ChatUtil.Confirm("Do you want to change the rental and try again?"))
            {
                throw new Exception("Rental Aborted!");
            }
            request = UpdateModelWithUserInput(request);
        }

        RentalController.GetInstance().Add(request);
    }

    private static void Return(string[] token)
    {
        if (token.Length > 1)
        {
            ChatUtil.PrintReturnHelp();
            throw new Exception("Invalid Syntax. Expecting at most one parameter");
        }

        // get rental
        int id;
        if (token.Length == 1)
        {
            id = int.Parse(token[0]);
        }
        else
        {
            RentalController.GetInstance().PrintAllActive();
            id = int.Parse(ChatUtil.GetInlineInput("Enter the rental id: "));
        }
        Rental rental = RentalController.GetInstance().GetByID(id);

        // print rental details
        Console.WriteLine(rental.GetDetailedHeader());
        Console.WriteLine(rental.GetDetailedValues().Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b));

        // check if rental is currently active
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        if (rental.StartDate >= today)
        {
            throw new Exception("This rental is not currently active!");
        }

        // check if rental is already returned
        if (rental.returned)
        {
            throw new Exception("This rental is already returned!");
        }

        // confirm return
        if (!ChatUtil.Confirm("Do you want to confirm the return of this rental?"))
        {
            throw new Exception("Return Aborted!");
        }

        // check if rental is overdue
        if (rental.EndDate < today && ChatUtil.Confirm("This rental is overdue. Do you want to change the end date?"))
        {
            rental.EndDate = today;
            Console.WriteLine("The end date has been updated to today.");
            Console.WriteLine("The new total price is " + rental.TotalPrice);
        }

        // mark rental as returned
        RentalController.GetInstance().Update(rental.ID, (Rental rental) => rental.returned = true);
    }
}
