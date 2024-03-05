using System.Diagnostics;

public class BoatController : AbstractController<Boat>
{
    private static BoatController? instance;

    public static BoatController GetInstance()
    {
        return instance ??= new BoatController();
    }
    public BoatController() : base("boats.xml") { }

    public Boat[] GetByCategoryID(int categoryId)
    {
        return _data.Where(boat => boat.CategoryID == categoryId).ToArray();
    }

    public void PrintBoatsForCategory(int categoryId)
    {
        Print(GetByCategoryID(categoryId));
    }
}