using System.Xml.Serialization;

public class Rental : AbstractModel<Rental>
{
    public int CustomerID { get; set; }

    public int BoatID { get; set; }

    [XmlIgnore]
    public DateOnly StartDate { get; set; }
    public string ProxyStartDate { get => StartDate.ToString(); set => StartDate = DateOnly.Parse(value); }

    [XmlIgnore]
    public DateOnly EndDate { get; set; }
    public string ProxyEndDate { get => EndDate.ToString(); set => EndDate = DateOnly.Parse(value); }

    public int Duration { get => EndDate.Day - StartDate.Day + 1; } // from today to today is 1 day; from today to tomorrow is 2 days

    public int TotalPrice
    {
        get
        {
            BoatCategory BoatCategory = BoatCategoryController.GetInstance().GetByID(BoatController.GetInstance().GetByID(BoatID).CategoryID);
            int totalPrice = Duration * BoatCategory.Price;
            // if the rental is longer than 2 days, the customer gets a category specific discount
            if (Duration > 2)
            {
                int discount = (int)(totalPrice * BoatCategory.LongtimeDiscount / 100.0);
                totalPrice -= discount;
            };
            return totalPrice;
        }
    } // in cents

    public bool returned { get; set; }

    public Rental()
    {
        ID = -1;
        CustomerID = -1;
        BoatID = -1;
        StartDate = DateOnly.FromDateTime(DateTime.Now);
        EndDate = StartDate.AddDays(1);
        returned = false;
    }

    public Rental(int id, int customerID, int boatID, DateOnly startDate, DateOnly endDate, bool returned)
    {
        ID = id;
        CustomerID = customerID;
        BoatID = boatID;
        StartDate = startDate;
        EndDate = endDate;
        returned = false;
    }

    public override Rental SetValues(params string[] values)
    {
        ID = int.Parse(values[0]);
        CustomerID = int.Parse(values[1]);
        BoatID = int.Parse(values[2]);
        StartDate = DateOnly.Parse(values[3]);
        EndDate = DateOnly.Parse(values[4]);
        return this;
    }

    public override string[] GetValues()
    {
        return new string[] { ID.ToString(), CustomerID.ToString(), BoatID.ToString(), StartDate.ToString(), EndDate.ToString() };
    }

    public override string[] GetDetailedValues()
    {
        return new string[] { ID.ToString(), CustomerID.ToString(), BoatID.ToString(), StartDate.ToString(), EndDate.ToString(), Duration.ToString(), TotalPrice.ToString(), returned.ToString() };
    }

    public override string GetHeader()
    {
        return new string[] { "ID", "CustomerID", "BoatID", "StartDate", "EndDate" }.Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b);
    }

    public override string GetDetailedHeader()
    {
        return new string[] { "ID", "CustomerID", "BoatID", "StartDate", "EndDate", "Duration", "TotalPrice", "Returned" }.Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b);
    }
}
