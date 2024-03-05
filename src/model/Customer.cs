public class Customer : AbstractModel<Customer>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Address Address { get; set; }
    public DateTime Birthday { get; set; }
    public string Iban { get; set; }
    public bool HasLicense { get; set; }

    public Customer()
    {
        ID = -1;
        FirstName = "";
        LastName = "";
        Address = new Address();
        Birthday = DateTime.Now;
        Iban = "";
        HasLicense = false;
    }

    public Customer(int id, string firstName, string lastName, Address address, DateTime birthday, string iban, bool hasLicense)
    {
        ID = id;
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        Birthday = birthday;
        Iban = iban;
        HasLicense = hasLicense;
    }

    public override Customer SetValues(params string[] values)
    {
        ID = int.Parse(values[0]);
        FirstName = values[1];
        LastName = values[2];
        Address = new Address(values[3]);
        Birthday = DateTime.Parse(values[4]);
        Iban = values[5];
        return this;
    }

    public override string[] GetValues()
    {
        return new string[] { ID.ToString(), FirstName, LastName, Address.ToString(), Birthday.ToString(), Iban, HasLicense.ToString() };
    }
    public override string GetHeader()
    {
        return new string[] {"ID", "Firstname", "Lastname", Address.GetHeader(), "Birthday", "IBAN", "Has License"}.Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b);
    }
}
