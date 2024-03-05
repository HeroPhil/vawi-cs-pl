public class BoatCategory : AbstractModel<BoatCategory>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; } // in cents (eg. 1000 = 10.00€)
    public int LongtimeDiscount { get; set; } // in percent points (eg. 10 = 10% discount)

    public BoatCategory()
    {
        ID = -1;
        Name = "";
        Description = "";
        Price = 0;
        LongtimeDiscount = 0;
    }

    public BoatCategory(int id, string name, string description, int price, int discount)
    {
        ID = id;
        Name = name;
        Description = description;
        Price = price;
        LongtimeDiscount = discount;
    }

    public override BoatCategory SetValues(params string[] values)
    {
        ID = int.Parse(values[0]);
        Name = values[1];
        Description = values[2];
        Price = int.Parse(values[3]);
        LongtimeDiscount = int.Parse(values[4]);
        return this;
    }

    public override string[] GetValues()
    {
        return new string[] { ID.ToString(), Name, Description, Price.ToString(), LongtimeDiscount.ToString() };
    }

    public override string GetHeader()
    {
        return new string[] {"ID", "Name", "Description", "Price per Day", "longtime Discount"}.Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b);
    }
}
