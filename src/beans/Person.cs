class Person : IDataBean<Person>
{
    public int ID { get; set; }
    public PersonTypEnum PersonTyp { get; set; }
    public string Vorname { get; set; }
    public string Nachname { get; set; }
    public Adresse Adresse { get; set; }
    public DateTime Geburtsdatum { get; set; }
    public string Abschluss { get; set; }

    public Person()
    {
    }

    public Person(PersonTypEnum personTyp, string vorname, string nachname, string abschluss, DateTime geburtsdatum, Adresse adresse)
    {
        PersonTyp = personTyp;
        Vorname = vorname;
        Nachname = nachname;
        Abschluss = abschluss;
        Geburtsdatum = geburtsdatum;
        Adresse = adresse;
    }

    public Person SetValues(params string[] values)
    {
        ID = int.Parse(values[0]);
        PersonTyp = Enum.Parse<PersonTypEnum>(values[1]);
        Vorname = values[2];
        Nachname = values[3];
        Adresse = new Adresse(values[4]);
        Geburtsdatum = DateTime.Parse(values[5]);
        Abschluss = values[6];
        return this;
    }

    public string[] GetValues()
    {
        return new string[] { ID.ToString(), PersonTyp.ToString(), Vorname, Nachname, Adresse.ToString(), Geburtsdatum.ToString(), Abschluss };
    }
}