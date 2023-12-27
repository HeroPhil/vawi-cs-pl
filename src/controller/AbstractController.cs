
using System.Xml.Serialization;

abstract class AbstractController<T> where T : DataBean<T>, new()
{
    public static string FieldDelimiter { get; } = "|";
    public static string SubFieldDelimiter { get; } = ":";

    public int NextFreeId()
    {
        return _data.Select(item => item.ID).DefaultIfEmpty(0).Max() + 1;
    }

    protected List<T> _data { get; private set; }

    private string _storagePath { get; }

    protected bool _autoSave { get; } = true;

    private XmlSerializer xmlSerializer { get; } = new XmlSerializer(typeof(List<T>));

    public AbstractController(string storagePath)
    {
        _data = new List<T>();
        _storagePath = storagePath;
    }


    public AbstractController<T> Load()
    {
        // Load data from file in _storagePath
        using (var parser = new FileStream(_storagePath, FileMode.OpenOrCreate))
        {
            _data = (List<T>)xmlSerializer.Deserialize(parser);
        }

        return this;
    }

    public AbstractController<T> Save()
    {
        // Save data to file in _storagePath
        using (var writer = new StreamWriter(_storagePath))
        {
            xmlSerializer.Serialize(writer, _data);
        }

        return this;
    }





}