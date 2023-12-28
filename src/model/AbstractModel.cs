public abstract class AbstractModel<T> {
    public int ID { get; set; }
    abstract public T SetValues(params string[] values);

    abstract public string[] GetValues();

    abstract public string ToString(); // only for documentation, toString is automatically implemented by object

    abstract public string GetHeader();
}