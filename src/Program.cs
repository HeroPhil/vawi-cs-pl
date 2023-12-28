using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Security.AccessControl;
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
        TeilnahmeController.GetInstance();
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
                case "assign":
                    Assign(commandToken.Skip(1).ToArray());
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
            // TODO throw exception
            ShowHelp();
            return;
        }

        try
        {
            switch (token[0])
            {
                case "person":
                    PersonController.GetInstance().Add(CreateBeanWithUserInput<Person>(PersonController.GetInstance().NextFreeId));
                    return;
                case "kurs":
                    KursController.GetInstance().Add(CreateBeanWithUserInput<Kurs>(KursController.GetInstance().NextFreeId));
                    return;
            }
        }
        catch (Exception e)
        {
            // TODO throw exception
            Console.WriteLine(e.Message);
        }
    }

    static T CreateBeanWithUserInput<T>(int id) where T : AbstractModel<T>, new()
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

    static void Assign(string[] token)
    {
        if (token.Length < 2)
        {
            // TODO throw exception
            ShowHelp();
            return;
        }

        try
        {
            int personID = int.Parse(token[0]);
            int kursID = int.Parse(token[1]);

            Person person = PersonController.GetInstance().GetByID(personID);
            if (person.PersonTyp != PersonTypEnum.Student)
            {
                throw new Exception("Only students can be assigned to a course!");
            }

            if (TeilnahmeController.GetInstance().GetByIDs(personID, kursID) != null)
            {
                throw new Exception($"The student with Id {personID} is already assigned to this course!");
            }

            TeilnahmeController.GetInstance().Add(new Teilnahme()
            {
                ID = TeilnahmeController.GetInstance().NextFreeId,
                PersonID = personID,
                KursID = kursID
            });
        }
        catch (Exception e)
        {
            // TODO throw exception
            Console.WriteLine(e.Message);
        }

    }
}
