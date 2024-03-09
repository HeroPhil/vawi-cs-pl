public class Boat : AbstractModel<Boat>
{
    public int CategoryID { get; set; }
    public string Brand { get; set; }
    public string Description { get; set; }
    public int Seats { get; set; }
    public bool RequiresLicense { get; set; }

    public bool IsAvailable
    {
        get
        {
            Rental[] rentals = RentalController.GetInstance().GetAllForBoat(ID);
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);

            foreach (Rental rental in rentals)
            {
                if (rental.StartDate <= now && rental.EndDate >= now)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public Boat()
    {
        ID = -1;
        CategoryID = -1;
        Brand = "";
        Description = "";
        Seats = 0;
        RequiresLicense = false;
    }

    public Boat(int id, int categoryID, string brand, string description, int seats, bool requiresLicense)
    {
        ID = id;
        CategoryID = categoryID;
        Brand = brand;
        Description = description;
        Seats = seats;
        RequiresLicense = requiresLicense;
    }

    public override Boat SetValues(params string[] values)
    {
        ID = int.Parse(values[0]);
        CategoryID = int.Parse(values[1]);
        Brand = values[2];
        Description = values[3];
        Seats = int.Parse(values[4]);
        RequiresLicense = bool.Parse(values[5]);
        return this;
    }

    public override string[] GetValues()
    {
        return new string[] { ID.ToString(), CategoryID.ToString(), Brand, Description, Seats.ToString(), RequiresLicense.ToString() };
    }

    public override string[] GetDetailedValues()
    {
        return new string[] { ID.ToString(), CategoryID.ToString(), Brand, Description, Seats.ToString(), RequiresLicense.ToString(), IsAvailable.ToString() };
    }

    public override string GetHeader()
    {
        return new string[] { "ID", "CategoryId", "Brand", "Description", "Seats", "Requires License" }.Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b);
    }

    public override string GetDetailedHeader()
    {
        return new string[] { "ID", "CategoryId", "Brand", "Description", "Seats", "Requires License", "Is Available" }.Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b);
    }
}
