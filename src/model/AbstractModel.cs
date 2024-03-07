public abstract class AbstractModel<T> where T : AbstractModel<T>, new()
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
    // Gets the detailed values of the model, which includes the values of the fields and some calculated values.
    // </summary>
    // <returns>The detailed values in the order dictated by the header</returns>
    virtual public string[] GetDetailedValues() {
        return GetValues();
    }

    // <summary>
    // Gets the header of the model, which includes the names of the fields.
    // </summary>
    // <returns>The header delimited by the ChatUtil.FieldDelimiter</returns>
    abstract public string GetHeader();

    // <summary>
    // Gets the detailed header of the model, which includes the names of the fields including for calculated values.
    // </summary>
    // <returns>The detailed header delimited by the ChatUtil.FieldDelimiter</returns>
    virtual public string GetDetailedHeader() {
        return GetHeader();
    }

    public override string ToString() => GetDetailedValues().Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b);

}