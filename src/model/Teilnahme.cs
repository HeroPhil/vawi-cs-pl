public class Teilnahme : AbstractModel<Teilnahme>
{
    public int PersonID { get; set; }
    public int KursID { get; set; }
    public float? Note { get; set; }

    public Teilnahme()
    {
        PersonID = -1;
        KursID = -1;
    }

    public Teilnahme(int personID, int kursID, float? note)
    {
        PersonID = personID;
        KursID = kursID;
        Note = note;
    }

    public override string GetHeader()
    {
        return new string[] { "PersonID", "KursID", "Note"}.Aggregate((a, b) => a + AbstractController<Teilnahme>.FieldDelimiter + b);
    }

    public override string[] GetValues()
    {
        return new string[] { PersonID.ToString(), KursID.ToString(), Note.ToString() };
    }

    public override string ToString()
    {
        return GetValues().Aggregate((a, b) => a + AbstractController<Teilnahme>.FieldDelimiter + b);
    }

    public override Teilnahme SetValues(params string[] values)
    {
        PersonID = int.Parse(values[0]);
        KursID = int.Parse(values[1]);
        Note = float.Parse(values[2]);
        return this;
    }

}