internal class Kurs : AbstractModel<Kurs>
{
    public string Name { get; set; }
    public string Beschreibung { get; set; }
    public string Semester { get; set; }
    public DateTime Startdatum { get; set; }
    public DateTime Enddatum { get; set; }
    public int DozentID { get; set; }

    public Kurs()
    {
        ID = -1;
        Name = "";
        Beschreibung = "";
        Semester = "";
        Startdatum = DateTime.Now;
        Enddatum = DateTime.Now;
        DozentID = -1;
    }

    public Kurs(string name, string beschreibung, string semester, DateTime startdatum, DateTime enddatum, int dozentID)
    {
        Name = name;
        Beschreibung = beschreibung;
        Semester = semester;
        Startdatum = startdatum;
        Enddatum = enddatum;
        DozentID = dozentID;
    }

    public override string GetHeader()
    {
        return new string[] { "ID", "Name", "Beschreibung", "Semester", "Startdatum", "Enddatum", "DozentId"}.Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b);
    }

    public override string[] GetValues()
    {
        return new string[] { ID.ToString(), Name, Beschreibung, Semester, Startdatum.ToString(), Enddatum.ToString(), DozentID.ToString() };
    }

    public override Kurs SetValues(params string[] values)
    {
        ID = int.Parse(values[0]);
        Name = values[1];
        Beschreibung = values[2];
        Semester = values[3];
        Startdatum = DateTime.Parse(values[4]);
        Enddatum = DateTime.Parse(values[5]);
        DozentID = int.Parse(values[6]);
        return this;
    }
}