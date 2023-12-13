class Adresse
{
    public string Strasse { get; set; }
    public string Hausnummer { get; set; }
    public string PLZ { get; set; }
    public string Ort { get; set; }
    public string Land { get; set; }

    public Adresse()
    {
        Strasse = "";
        Hausnummer = "";
        PLZ = "";
        Ort = "";
        Land = "";
    }

    public Adresse(string strasse, string hausnummer, string plz, string ort, string land)
    {
        Strasse = strasse;
        Hausnummer = hausnummer;
        PLZ = plz;
        Ort = ort;
        Land = land;
    }

    public Adresse(string value) {
        string[] values = value.Split(AbstractController<Person>.SubFieldDelimiter);
        Strasse = values[0];
        Hausnummer = values[1];
        PLZ = values[2];
        Ort = values[3];
        Land = values[4];
    }

    public override string ToString()
    {
        return new string[] { Strasse, Hausnummer, PLZ, Ort, Land }.Aggregate((a, b) => a + AbstractController<Person>.SubFieldDelimiter + b);
    }

    public string GetHeader()
    {
        return new string[] {"Strasse", "Hausnummer", "PLZ", "Ort", "Land"}.Aggregate((a, b) => a + AbstractController<Person>.SubFieldDelimiter + b);
    }   

}