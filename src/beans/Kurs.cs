class Kurs : DataBean<Kurs>
{
    string Name { get; set; }
    string Beschreibung { get; set; }
    string Semester { get; set; }
    DateTime Startdatum { get; set; }
    DateTime Enddatum { get; set; }

    public override string GetHeader()
    {
        return new string[] { "ID", "Name", "Beschreibung", "Semester", "Startdatum", "Enddatum" }.Aggregate((a, b) => a + AbstractController<Kurs>.PrintFieldDelimiter + b);
    }

    public override string[] GetValues()
    {
        return new string[] { ID.ToString(), Name, Beschreibung, Semester, Startdatum.ToString(), Enddatum.ToString() };
    }

    public override string ToString()
    {
        return GetValues().Aggregate((a, b) => a + AbstractController<Kurs>.PrintFieldDelimiter + b);
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