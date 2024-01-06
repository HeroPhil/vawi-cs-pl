internal class TeilnahmeController : AbstractController<Teilnahme>
{
    private static TeilnahmeController? instance;

    public static TeilnahmeController GetInstance()
    {
        return instance ??= new TeilnahmeController();
    }

    public TeilnahmeController() : base("teilnahme.xml") {}

    public Teilnahme? GetByIDs(int personID, int kursID)
    {
        IEnumerable<Teilnahme> queryResult = from item in _data
                                             where item.PersonID == personID && item.KursID == kursID
                                             select item;

        if (queryResult.Count() == 0)
        {
            return null;
        }

        return queryResult.First();
    }

    public Teilnahme[] GetAllForPerson(int personID)
    {
        IEnumerable<Teilnahme> queryResult = from item in _data
                                             where item.PersonID == personID
                                             select item;

        return queryResult.ToArray();
    }

    public Teilnahme[] GetAllForKurs(int kursID)
    {
        IEnumerable<Teilnahme> queryResult = from item in _data
                                             where item.KursID == kursID
                                             select item;

        return queryResult.ToArray();
    }

    public void PrintAllForPerson(int personID)
    {
        Print(GetAllForPerson(personID));
    }

    public void PrintAllForKurs(int kursID)
    {
        Print(GetAllForKurs(kursID));
    }
}