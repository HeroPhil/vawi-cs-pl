class KursController : AbstractController<Kurs>
{

    static KursController? instance;

    public static KursController GetInstance()
    {
        return instance ??= new KursController();
    }

    public KursController() : base("kurse.xml") { }

    public Kurs[] GetByDozentId(int id)
    {
        IEnumerable<Kurs> query = from kurs in GetAll()
                                 where kurs.DozentID == id
                                 select kurs;

        return query.ToArray();
    }

}