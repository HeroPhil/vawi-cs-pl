internal abstract class AbstractModel<T> where T : AbstractModel<T>, new()
{
    // <summary>
    // The ID of the model.
    // </summary>
    public int ID { get; set; }

    // <summary>
    // Sets the values of the model.
    // </summary>
    // <param name="values">The values in the order dictated by the header</param>
    abstract public T SetValues(params string[] values);

    // <summary>
    // Gets the values of the model.
    // </summary>
    // <returns>The values in the order dictated by the header</returns>
    abstract public string[] GetValues();

    // <summary>
    // Gets the header of the model, which includes the names of the fields.
    // </summary>
    // <returns>The header delimited by the ChatUtil.FieldDelimiter</returns>
    abstract public string GetHeader();

    public override string ToString() => GetValues().Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b);

}