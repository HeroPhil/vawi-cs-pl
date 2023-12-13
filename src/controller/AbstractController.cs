
using Microsoft.VisualBasic.FileIO;

abstract class AbstractController<T> where T : DataBean<T>, new()
{
    public static string FileFieldDelimiter { get; } = ",";
    public static string PrintFieldDelimiter { get; } = "|";
    public static string SubFieldDelimiter { get; } = ":";

    protected List<T> _data { get; }

    private string _storagePath { get; }

    protected bool _autoSave { get; } = true;

    public int NextFreeId
    {
        get
        {
            return _data.Select(item => item.ID).DefaultIfEmpty(0).Max() + 1;
        }
    }

    public AbstractController(string storagePath)
    {
        _data = new List<T>();
        _storagePath = storagePath;

        if (!File.Exists(_storagePath))
        {
            File.Create(_storagePath);
        }
    }


    public AbstractController<T> Load()
    {
        // Load data from file in _storagePath
        using (var parser = new TextFieldParser(_storagePath))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            while (!parser.EndOfData)
            {
                // Process row
                string[]? fields = parser.ReadFields();
                if (fields != null)
                {
                    _data.Add(new T().SetValues(fields));
                }
            }
        }

        return this;
    }

    public AbstractController<T> Save()
    {
        // Save data to file in _storagePath
        using (var writer = new StreamWriter(_storagePath))
        {
            foreach (var item in _data)
            {
                writer.WriteLine(item.GetValues().Aggregate((a, b) => a + FileFieldDelimiter + b));
            }
        }

        return this;
    }





}