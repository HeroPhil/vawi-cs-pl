using System.Collections;
using System.ComponentModel;
using System.Data;
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

            string[] token = commandToken.Skip(1).ToArray();
            switch (commandToken[0].ToLower())
            {
                case "ls":
                    ListAll(token);
                    continue;
                case "add":
                    Add(token);
                    continue;
                case "update":
                    Update(token);
                    continue;
                case "rm":
                    Remove(token);
                    continue;
                case "save":
                    PersonController.GetInstance().Save();
                    KursController.GetInstance().Save();
                    TeilnahmeController.GetInstance().Save();
                    continue;
                case "assign":
                    Assign(token);
                    continue;
                case "dismiss":
                    Dismiss(token);
                    continue;
                case "grade":
                    Grade(token);
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

        if (token.Length == 2)
        {
            ListFilter(token);
            return;
        }

        switch (token[0])
        {
            case "person":
                PersonController.GetInstance().PrintAll();
                return;
            case "kurs":
                KursController.GetInstance().PrintAll();
                return;
        }

    }

    static void ListFilter(string[] token)
    {
        int id = int.Parse(token[1]);
        switch (token[0])
        {
            case "person":
                TeilnahmeController.GetInstance().PrintAllForPerson(id);
                return;
            case "kurs":
                TeilnahmeController.GetInstance().PrintAllForKurs(id);
                return;
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
                    PersonController.GetInstance().Add(CreateModelWithUserInput<Person>(PersonController.GetInstance().NextFreeId));
                    return;
                case "kurs":
                    KursController.GetInstance().Add(CreateModelWithUserInput<Kurs>(KursController.GetInstance().NextFreeId));
                    return;
            }
        }
        catch (Exception e)
        {
            // TODO throw exception
            Console.WriteLine(e.Message);
        }
    }

    static void Update(string[] token)
    {
        if (token.Length != 2)
        {
            ShowHelp();
            return;
        }

        int id = int.Parse(token[1]);
        switch (token[0])
        {
            case "person":
                PersonController.GetInstance().Update(id, (Person person) => UpdateModelWithUserInput(person));
                return;
            case "kurs":
                KursController.GetInstance().Update(id, (Kurs kurs) => UpdateModelWithUserInput(kurs));
                return;
        }
    }

    static T CreateModelWithUserInput<T>(int id) where T : AbstractModel<T>, new()
    {
        T model = new T();
        model.ID = id;
        return UpdateModelWithUserInput(model);
    }

    static T UpdateModelWithUserInput<T>(T model) where T : AbstractModel<T>, new()
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
            throw new Exception("Adding Aborted!");
        }

        // return final model
        return model.SetValues(values.ToArray());
    }

    private static void Remove(string[] token)
    {
        if (token.Length != 2)
        {
            ShowHelp();
            return;
        }

        int id = int.Parse(token[1]);
        switch (token[0])
        {
            case "person":
                if (TeilnahmeController.GetInstance().GetAllForPerson(id).Length > 0)
                {
                    throw new Exception("Cannot remove a person that is assigned to a course!");
                }
                PersonController.GetInstance().Remove(id);
                return;
            case "kurs":
                if (TeilnahmeController.GetInstance().GetAllForKurs(id).Length > 0)
                {
                    throw new Exception("Cannot remove a course that has students assigned!");
                }
                KursController.GetInstance().Remove(id);
                return;
            default:
                throw new Exception($"Unknown model type \"{token[0]}\"!");
        }
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

    static void Dismiss(string[] token)
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
                throw new Exception("Only students can be dismissed from a course");
            }

            Kurs kurs = KursController.GetInstance().GetByID(kursID);

            Teilnahme? teilnahme = TeilnahmeController.GetInstance().GetByIDs(personID, kursID) ?? throw new Exception($"The student with Id {personID} is not assigned to this course!");

            if (!ChatUtil.Confirm($"Do you want to dismiss the student \"{person.Vorname} {person.Nachname}\" from course \"{kurs.Name}\" ({kurs.Semester})?{(teilnahme.Note == null ? "" : $"\n\tAll grades will be deleted!")}"))
            {
                throw new Exception("Dismissal aborted!");
            }

            TeilnahmeController.GetInstance().Remove(teilnahme);
        }
        catch (Exception e)
        {
            // TODO throw exception
            Console.WriteLine(e.Message);
        }
    }

    static void Grade(string[] token)
    {
        if (token.Length < 2)
        {
            // TODO throw exception
            ShowHelp();
            return;
        }

        if (token.Length == 3)
        {
            // directly grade one student in one course
            GradeStudentInCourse(int.Parse(token[0]), int.Parse(token[1]), float.Parse(token[2]));
            return;
        }

        int id = int.Parse(token[1]);
        Teilnahme[]? teilnahmen;
        switch (token[0])
        {
            // grade all students in one course
            case "kurs":
                teilnahmen = TeilnahmeController.GetInstance().GetAllForKurs(id);
                break;
            // grade one student in all courses
            case "student":
                teilnahmen = TeilnahmeController.GetInstance().GetAllForPerson(id);
                break;
            // grade one student in one course
            default:
                GradeStudentInCourse(int.Parse(token[0]), id);
                return;
        }

        foreach (Teilnahme teilnahme in teilnahmen)
        {
            GradeStudentInCourse(teilnahme.PersonID, teilnahme.KursID);
        }
    }

    private static void GradeStudentInCourse(int personID, int kursID)
    {
        Person person = PersonController.GetInstance().GetByID(personID);
        Kurs kurs = KursController.GetInstance().GetByID(kursID);

        float note = float.Parse(ChatUtil.GetInput($"Enter grade for student \"{person.Vorname} {person.Nachname}\" in course \"{kurs.Name}\" ({kurs.Semester})"));

        GradeStudentInCourse(personID, kursID, note);
    }

    private static void GradeStudentInCourse(int personID, int kursID, float note)
    {
        Person person = PersonController.GetInstance().GetByID(personID);
        if (person.PersonTyp != PersonTypEnum.Student)
        {
            throw new Exception("Only students can be graded!");
        }

        Kurs kurs = KursController.GetInstance().GetByID(kursID);

        Teilnahme? teilnahme = TeilnahmeController.GetInstance().GetByIDs(personID, kursID) ?? throw new Exception($"The student with Id {personID} is not assigned to this course!");

        if (teilnahme.Note != null && !ChatUtil.Confirm($"The student \"{person.Vorname} {person.Nachname}\" already has the grade {teilnahme.Note} for course \"{kurs.Name}\" ({kurs.Semester})!\nDo you want to overwrite the grade?"))
        {
            throw new Exception("Grading aborted!");
        }

        TeilnahmeController.GetInstance().Update(teilnahme.ID, (Teilnahme teilnahme) => teilnahme.Note = note);
    }

}
