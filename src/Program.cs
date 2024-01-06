internal class Program
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
        PersonController.GetInstance();
        KursController.GetInstance();
        TeilnahmeController.GetInstance();
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
                        PersonController.GetInstance().Save();
                        KursController.GetInstance().Save();
                        TeilnahmeController.GetInstance().Save();
                        continue;
                    case "assign":
                        Assign(parameters);
                        continue;
                    case "dismiss":
                        Dismiss(parameters);
                        continue;
                    case "grade":
                        Grade(parameters);
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
            case "person":
                TeilnahmeController.GetInstance().PrintAllForPerson(id);
                return;
            case "kurs":
                TeilnahmeController.GetInstance().PrintAllForKurs(id);
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
            case "person":
                PersonController.GetInstance().Add(CreateModelWithUserInput<Person>(PersonController.GetInstance().NextFreeId));
                return;
            case "kurs":
                KursController.GetInstance().Add(CreateModelWithUserInput<Kurs>(KursController.GetInstance().NextFreeId));
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
                PersonController.GetInstance().Update(id, (Person person) => UpdateModelWithUserInput(person));
                return;
            case "kurs":
                KursController.GetInstance().Update(id, (Kurs kurs) => UpdateModelWithUserInput(kurs));
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
            case "person":
                if (TeilnahmeController.GetInstance().GetAllForPerson(id).Length > 0)
                {
                    throw new Exception("Cannot remove a student that is assigned to a course!");
                }
                if (KursController.GetInstance().GetByDozentId(id).Length > 0)
                {
                    throw new Exception("Cannot remove a dozent that is assigned to a course!");
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

    // <summary>
    // Assigns a student to a course.
    // </summary>
    // <param name="token">The command token.</param>
    private static void Assign(string[] token)
    {
        if (token.Length != 2)
        {
            ChatUtil.PrintAssignHelp();
            throw new Exception("Invalid Syntax. Expecting exactly two parameters");
        }


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

    // <summary>
    // Dismisses a student from a course.
    // </summary>
    // <param name="token">The command token.</param>
    private static void Dismiss(string[] token)
    {
        if (token.Length != 2)
        {
            ChatUtil.PrintDismissHelp();
            throw new Exception("Invalid Syntax. Expecting exactly two parameters");
        }


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

    // <summary>
    // Grades a student in a course.
    // </summary>
    // <param name="token">The command token.</param>
    private static void Grade(string[] token)
    {
        if (token.Length < 2)
        {
            ChatUtil.PrintGradeHelp();
            throw new Exception("Invalid Syntax. Expecting at least two parameters");
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

    // <summary>
    // Grades a student in a course with user input.
    // </summary>
    // <param name="personID">The id of the student.</param>
    // <param name="kursID">The id of the course.</param>
    private static void GradeStudentInCourse(int personID, int kursID)
    {
        Person person = PersonController.GetInstance().GetByID(personID);
        Kurs kurs = KursController.GetInstance().GetByID(kursID);

        float note = float.Parse(ChatUtil.GetInput($"Enter grade for student \"{person.Vorname} {person.Nachname}\" in course \"{kurs.Name}\" ({kurs.Semester})"));

        GradeStudentInCourse(personID, kursID, note);
    }

    // <summary>
    // Grades a student in a course.
    // </summary>
    // <param name="personID">The id of the student.</param>
    // <param name="kursID">The id of the course.</param>
    // <param name="note">The grade.</param>
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
