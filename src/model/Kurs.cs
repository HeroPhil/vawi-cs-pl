public class Kurs : AbstractModel<Kurs>
{
    public string Name { get; set; }
    public string Beschreibung { get; set; }
    public string Semester { get; set; }
    public DateTime Startdatum { get; set; }
    public DateTime Enddatum { get; set; }

    public Kurs()
    {
        ID = -1;
        Name = "";
        Beschreibung = "";
        Semester = "";
        Startdatum = DateTime.Now;
        Enddatum = DateTime.Now;
    }

    public Kurs(string name, string beschreibung, string semester, DateTime startdatum, DateTime enddatum)
    {
        Name = name;
        Beschreibung = beschreibung;
        Semester = semester;
        Startdatum = startdatum;
        Enddatum = enddatum;
    }

    public override string GetHeader()
    {
        return new string[] { "ID", "Name", "Beschreibung", "Semester", "Startdatum", "Enddatum" }.Aggregate((a, b) => a + AbstractController<Kurs>.FieldDelimiter + b);
    }

    public override string[] GetValues()
    {
        return new string[] { ID.ToString(), Name, Beschreibung, Semester, Startdatum.ToString(), Enddatum.ToString() };
    }

    public override string ToString()
    {
        return GetValues().Aggregate((a, b) => a + AbstractController<Kurs>.FieldDelimiter + b);
    }

    public override Kurs SetValues(params string[] values)
    {
        ID = int.Parse(values[0]);
        Name = values[1];
        Beschreibung = values[2];
        Semester = values[3];
        Startdatum = DateTime.Parse(values[4]);
        Enddatum = DateTime.Parse(values[5]);
        return this;
    }
}