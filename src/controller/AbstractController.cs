
using System.Xml.Serialization;

public abstract class AbstractController<T> where T : AbstractModel<T>, new()
{
    public static string FieldDelimiter { get; } = "|";
    public static string SubFieldDelimiter { get; } = ":";

    protected static string baseStoragePath { get; } = "/data/";

    public int NextFreeId => _data.Select(item => item.ID).DefaultIfEmpty(0).Max() + 1;

    protected List<T> _data { get; private set; }

    protected bool AutoSave { get; } = true;

    private string StoragePath { get; }

    private XmlSerializer xmlSerializer { get; } = new XmlSerializer(typeof(List<T>));

    public AbstractController(string filename)
    {
        StoragePath = Environment.CurrentDirectory + baseStoragePath + filename;
        _data = new List<T>();
        if (new FileInfo(StoragePath).Exists)
            // load existing data
            Load();
        else
        {
            // init new file
            Save();
        }
    }


    public AbstractController<T> Load()
    {
        // Load data from file in _storagePath
        using (var parser = new FileStream(StoragePath, FileMode.Open))
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

    public AbstractController<T> Add(T item)
    {
        _data.Add(item);
        if (AutoSave) Save();
        return this;
    }

    public void PrintAll()
    {
        Console.WriteLine(new T().GetHeader()); // static not possible with interface
        foreach (T item in _data)
        {
            Console.WriteLine(item.ToString());
        }
    }

    public T GetByID(int id)
    {
        IEnumerable<T> queryResult = from item in _data
                                     where item.ID == id
                                     select item;

        if (queryResult.Count() == 0) throw new Exception($"No {typeof(T).Name} with ID {id} found!");

        return queryResult.First();
    }

}