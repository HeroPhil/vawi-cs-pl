class KursController : AbstractController<Kurs>
{

    static KursController? instance;

    public static KursController GetInstance()
    {
        return instance ??= new KursController();
    }

    public KursController() : base(Environment.CurrentDirectory + "/data/kurse.xml")
    {
        Load();
    }

    public KursController Add(Kurs kurs)
    {
        _data.Add(kurs);
        if (AutoSave) Save();
        return this;
    }

    public void PrintAll()
    {
        Console.WriteLine(new Kurs().GetHeader()); // static not possible with interface
        foreach (Kurs kurs in _data)
        {
            Console.WriteLine(kurs.ToString());
        }
    }


}