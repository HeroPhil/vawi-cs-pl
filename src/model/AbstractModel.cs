public abstract class AbstractModel<T> where T : AbstractModel<T>, new()
{
    public int ID { get; set; }
    abstract public T SetValues(params string[] values);
    abstract public string[] GetValues();
    abstract public string GetHeader();
    public override string ToString() => GetValues().Aggregate((a, b) => a + ChatUtil.FieldDelimiter + b);

}