using System.Diagnostics;

public class RentalController : AbstractController<Rental>
{
    private static RentalController? instance;

    public static RentalController GetInstance()
    {
        return instance ??= new RentalController();
    }

    // <summary>
    // Gets all rentals for a given customer ID.
    // </summary>
    // <param name="customerID">The customer ID.</param>
    // <returns>The rentals.</returns>
    public Rental[] GetAllForCustomer(int customerID)
    {
        IEnumerable<Rental> queryResult = from item in _data
                                          where item.CustomerID == customerID
                                          select item;

        return queryResult.ToArray();
    }


    // <summary>
    // Gets all rentals for a given boat ID.
    // </summary>
    // <param name="boatID">The boat ID.</param>
    // <returns>The rentals.</returns>

    public Rental[] GetAllForBoat(int boatID)
    {
        IEnumerable<Rental> queryResult = from item in _data
                                          where item.BoatID == boatID
                                          select item;

        return queryResult.ToArray();
    }

    public void PrintRentalsForCustomer(int id)
    {
        Print(GetAllForCustomer(id));
    }

    public void PrintRentalsForBoat(int id)
    {
        Print(GetAllForBoat(id));
    }

    public static Boolean validateRental(Rental rental)
    {
        // customer and boat must be set
        if (rental.CustomerID < 1 || rental.BoatID < 1)
        {
            Console.WriteLine("Customer and boat must be set");
            return false;
        }

        // customer must exist
        if (CustomerController.GetInstance().GetByID(rental.CustomerID) == null)
        {
            Console.WriteLine("Customer does not exist");
            return false;
        }

        // boat must exist
        if (BoatController.GetInstance().GetByID(rental.BoatID) == null)
        {
            Console.WriteLine("Boat does not exist");
            return false;
        }

        // start date must be before or at end date
        if (rental.StartDate > rental.EndDate)
        {
            Console.WriteLine("Start date must be before or at end date");
            return false;
        }

        // warning if start date is in the past
        if (rental.StartDate < DateOnly.FromDateTime(DateTime.Now))
        {
            Console.WriteLine("Warning: Start date is in the past");
            // no return false here, because this is just a warning
        }

        // duration must be at least 1 day
        if (rental.Duration < 1)
        {
            Console.WriteLine("Duration must be at least 1 day");
            return false;
        }

        // rental must not overlap with other rentals
        foreach (Rental otherRental in GetInstance().GetAll())
        {
            if (rental.ID != otherRental.ID && rental.BoatID == otherRental.BoatID)
            {
                if (rental.StartDate < otherRental.EndDate && rental.EndDate > otherRental.StartDate)
                {
                    Console.WriteLine("Rental overlaps with other rental");
                    return false;
                }
            }
        }
        return true;
    }

    public RentalController() : base("rentals.xml") { }



}