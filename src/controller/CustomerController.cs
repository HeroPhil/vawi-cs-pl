public class CustomerController : AbstractController<Customer>
{
    private static CustomerController? instance;

    public static CustomerController GetInstance()
    {
        return instance ??= new CustomerController();
    }
    public CustomerController() : base("customer.xml") { }
}