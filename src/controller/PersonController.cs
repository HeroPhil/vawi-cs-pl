class PersonController : AbstractController<Person>
{
    static PersonController? instance;

    public static PersonController GetInstance()
    {
        return instance ??= new PersonController();
    }
    public PersonController() : base(Environment.CurrentDirectory + "/data/person.csv")
    {
        Load();
    }

    public PersonController Add(Person person)
    {
        _data.Add(person);
        if (_autoSave) Save();
        return this;
    }

    public void PrintAll()
    {
        Console.WriteLine(new Person().GetHeader()); // static not possible with interface
        foreach (Person item in _data)
        {
            Console.WriteLine(item.ToString());
        }
    }


}