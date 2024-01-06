public class PersonController : AbstractController<Person>
{
    private static PersonController? instance;

    public static PersonController GetInstance()
    {
        return instance ??= new PersonController();
    }
    public PersonController() : base("personen.xml") { }
}