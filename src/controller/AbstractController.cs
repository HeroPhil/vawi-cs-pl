
using System.Xml.Serialization;

public abstract class AbstractController<T> where T : AbstractModel<T>, new()
{
    protected static string baseStoragePath { get; } = "/data/";

    public int NextFreeId => _data.Select(item => item.ID).DefaultIfEmpty(0).Max() + 1;

    protected List<T> _data { get; private set; }

    protected bool AutoSave { get; } = true;

    private string StoragePath { get; }

    private XmlSerializer xmlSerializer { get; } = new XmlSerializer(typeof(List<T>));

    // Constructor
    // <param name="filename">Filename of the storage file. The file will be created if it does not exist.</param>
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


    // <summary>
    // Loads the data from the storage file.
    // </summary>
    // <returns>The controller instance.</returns>
    public AbstractController<T> Load()
    {
        // Load data from file in _storagePath
        using (var parser = new FileStream(StoragePath, FileMode.Open))
        {
            _data = (List<T>)(xmlSerializer.Deserialize(parser) ?? new List<T>());
        }

        return this;
    }

    // <summary>
    // Saves the data to the storage file.
    // </summary>
    // <returns>The controller instance.</returns>
    public AbstractController<T> Save()
    {
        // Save data to file in _storagePath
        using (var writer = new StreamWriter(StoragePath))
        {
            xmlSerializer.Serialize(writer, _data);
        }

        return this;
    }

    // <summary>
    // Adds an item to the data list.
    // </summary>
    // <param name="item">The item to add.</param>
    // <returns>The controller instance.</returns>
    public virtual AbstractController<T> Add(T item)
    {
        _data.Add(item);
        if (AutoSave) Save();
        return this;
    }

    // <summary>
    // Removes an item from the data list.
    // </summary>
    // <param name="item">The item to remove.</param>
    // <returns>The controller instance.</returns>
    public AbstractController<T> Remove(T item)
    {
        _data.Remove(item);
        if (AutoSave) Save();
        return this;
    }

    // <summary>
    // Removes an item from the data list by ID.
    // </summary>
    // <param name="id">The ID of the item to remove.</param>
    // <returns>The controller instance.</returns>
    public AbstractController<T> Remove(int id)
    {
        _data.Remove(GetByID(id));
        if (AutoSave) Save();
        return this;
    }

    // <summary>
    // Updates an item in the data list.
    // </summary>
    // <param name="item">The item to update.</param>
    // <returns>The controller instance.</returns>
    public AbstractController<T> Update(int id, Action<T> updateFunction)
    {
        // This ensures that the item exists and is part of the data list. Object is safely mutated.
        updateFunction(GetByID(id)); 
        if (AutoSave) Save();
        return this;
    }

    // <summary>
    // Returns all items in the data list.
    // </summary>
    // <returns>All items in the data list.</returns>
    public T[] GetAll()
    {
        return _data.ToArray();
    }

    // <summary>
    // Prints all items in the data list.
    // </summary>
    public void PrintAll()
    {
        Print(GetAll());
    }

    // <summary>
    // Returns an item by ID.
    // </summary>
    // <param name="id">The ID of the item to return.</param>
    // <returns>The item with the given ID.</returns>
    // <exception cref="Exception">Thrown when no item with the given ID exists.</exception>
    public T GetByID(int id)
    {
        IEnumerable<T> queryResult = from item in _data
                                     where item.ID == id
                                     select item;

        if (queryResult.Count() == 0) throw new Exception($"No {typeof(T).Name} with ID {id} found!");

        return queryResult.First();
    }

    // <summary>
    // Prints an array of items.
    // </summary>
    // <param name="values">The items to print.</param>
    protected void Print(T[] values) {
        Console.WriteLine(new T().GetHeader()); // static not possible with interface
        foreach (T item in values)
        {
            Console.WriteLine(item.ToString());
        }
    }

}