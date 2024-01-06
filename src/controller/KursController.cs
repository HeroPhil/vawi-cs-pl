public class KursController : AbstractController<Kurs>
{
    private static KursController? instance;

    public static KursController GetInstance()
    {
        return instance ??= new KursController();
    }

    public KursController() : base("kurse.xml") { }

    // <summary>
    // Adds a new kurs to the data list.
    // Validates that the Dozent exists.
    // </summary>
    // <param name="kurs">The kurs to add.</param>
    // <returns>The controller instance.</returns>
    public override AbstractController<Kurs> Add(Kurs kurs)
    {
        if (PersonController.GetInstance().GetByID(kurs.DozentID).PersonTyp != PersonTypEnum.Dozent) {
            throw new Exception("The person with the ID " + kurs.DozentID + " is not a dozent.");
        }
        return base.Add(kurs);
    }

    // <summary>
    // Returns all kurs by a dozent.
    // </summary>
    // <param name="id">The ID of the dozent.</param>
    // <returns>All kurs by the dozent.</returns>
    public Kurs[] GetByDozentId(int id)
    {
        IEnumerable<Kurs> query = from kurs in GetAll()
                                  where kurs.DozentID == id
                                  select kurs;

        return query.ToArray();
    }

}