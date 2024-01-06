internal class TeilnahmeController : AbstractController<Teilnahme>
{
    private static TeilnahmeController? instance;

    public static TeilnahmeController GetInstance()
    {
        return instance ??= new TeilnahmeController();
    }

    public TeilnahmeController() : base("teilnahme.xml") {}

    // <summary>
    // Adds a new teilnahme to the data list.
    // Validates that the person and kurs exist.
    // </summary>
    // <param name="teilnahme">The teilnahme to add.</param>
    // <returns>The assignment or null if assignment exists.</returns>
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

    // <summary>
    // Gets all teilnahmen for a given person ID.
    // </summary>
    // <param name="personID">The person ID.</param>
    // <returns>The teilnahmen.</returns>
    public Teilnahme[] GetAllForPerson(int personID)
    {
        IEnumerable<Teilnahme> queryResult = from item in _data
                                             where item.PersonID == personID
                                             select item;

        return queryResult.ToArray();
    }

    // <summary>
    // Gets all teilnahmen for a given kurs ID.
    // </summary>
    // <param name="kursID">The kurs ID.</param>
    // <returns>The teilnahmen.</returns>
    public Teilnahme[] GetAllForKurs(int kursID)
    {
        IEnumerable<Teilnahme> queryResult = from item in _data
                                             where item.KursID == kursID
                                             select item;

        return queryResult.ToArray();
    }

    // <summary>
    // Prints all teilnahmen for a given person ID.
    // </summary>
    // <param name="personID">The person ID.</param>
    public void PrintAllForPerson(int personID)
    {
        Print(GetAllForPerson(personID));
    }

    // <summary>
    // Prints all teilnahmen for a given kurs ID.
    // </summary>
    public void PrintAllForKurs(int kursID)
    {
        Print(GetAllForKurs(kursID));
    }
}