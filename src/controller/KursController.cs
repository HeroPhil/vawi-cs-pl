class KursController : AbstractController<Kurs>
{

    static KursController? instance;

    public static KursController GetInstance()
    {
        return instance ??= new KursController();
    }

    public KursController() : base("kurse.xml") { }

    public override AbstractController<Kurs> Add(Kurs kurs)
    {
        if (PersonController.GetInstance().GetByID(kurs.DozentID).PersonTyp != PersonTypEnum.Dozent) {
            throw new Exception("The person with the ID " + kurs.DozentID + " is not a dozent.");
        }
        return base.Add(kurs);
    }

    public Kurs[] GetByDozentId(int id)
    {
        IEnumerable<Kurs> query = from kurs in GetAll()
                                  where kurs.DozentID == id
                                  select kurs;

        return query.ToArray();
    }

}