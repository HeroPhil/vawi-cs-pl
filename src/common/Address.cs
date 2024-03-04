public class Address
{
    public string Street { get; set; }
    public string Housenumber { get; set; }
    public string ZIP { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    public Address()
    {
        Street = "";
        Housenumber = "";
        ZIP = "";
        City = "";
        Country = "";
    }

    public Address(string strasse, string hausnummer, string plz, string ort, string land)
    {
        Street = strasse;
        Housenumber = hausnummer;
        ZIP = plz;
        City = ort;
        Country = land;
    }

    public Address(string value) {
        string[] values = value.Split(ChatUtil.SubFieldDelimiter);
        Street = values[0];
        Housenumber = values[1];
        ZIP = values[2];
        City = values[3];
        Country = values[4];
    }

    public override string ToString()
    {
        return new string[] { Street, Housenumber, ZIP, City, Country }.Aggregate((a, b) => a + ChatUtil.SubFieldDelimiter + b);
    }

    public string GetHeader()
    {
        return new string[] {"Street", "Housenumber", "ZIP", "City", "Country"}.Aggregate((a, b) => a + ChatUtil.SubFieldDelimiter + b);
    }   

}