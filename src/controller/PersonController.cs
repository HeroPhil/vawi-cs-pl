class PersonController : DataStore<Person>
{
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
        foreach (var item in _data)
        {
            Console.WriteLine(item);
        }
    }


}