class KursController : AbstractController<Kurs>
{

    static KursController? instance;

    public static KursController GetInstance()
    {
        return instance ??= new KursController();
    }

    public KursController() : base("kurse.xml") { }

}