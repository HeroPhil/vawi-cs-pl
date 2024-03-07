using System.Diagnostics;

public class BoatCategoryController : AbstractController<BoatCategory>
{
    private static BoatCategoryController? instance;

    public static BoatCategoryController GetInstance()
    {
        return instance ??= new BoatCategoryController();
    }
    public BoatCategoryController() : base("boat_categories.xml") { }
}