
using System.Xml.Serialization;

public abstract class AbstractController<T> where T : AbstractModel<T>, new()
{
    public static string FieldDelimiter { get; } = "|";
    public static string SubFieldDelimiter { get; } = ":";

    public int NextFreeId => _data.Select(item => item.ID).DefaultIfEmpty(0).Max() + 1;

    protected List<T> _data { get; private set; }

    protected bool AutoSave { get; } = true;

    private string StoragePath { get; }

    private XmlSerializer xmlSerializer { get; } = new XmlSerializer(typeof(List<T>));

    public AbstractController(string storagePath)
    {
        StoragePath = storagePath;
    }


    public AbstractController<T> Load()
    {
        // Load data from file in _storagePath
        using (var parser = new FileStream(StoragePath, FileMode.OpenOrCreate))
        {
            _data = (List<T>)xmlSerializer.Deserialize(parser);
        }

        return this;
    }

    public AbstractController<T> Save()
    {
        // Save data to file in _storagePath
        using (var writer = new StreamWriter(StoragePath))
        {
            xmlSerializer.Serialize(writer, _data);
        }

        return this;
    }

}