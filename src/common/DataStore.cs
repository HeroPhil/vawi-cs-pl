
using System.Collections;
using Microsoft.VisualBasic.FileIO;

class DataStore<T> where T : IDataBean<T>, new()
{
    protected List<T> _data { get; }

    private string _storagePath { get; }

    protected bool _autoSave { get; } = true;

    public DataStore(string storagePath)
    {
        _data = new List<T>();
        _storagePath = storagePath;

        if (!File.Exists(_storagePath))
        {
            File.Create(_storagePath);
        }
    }


    public DataStore<T> Load()
    {
        // Load data from file in _storagePath
        using (var parser = new TextFieldParser(_storagePath))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            while (!parser.EndOfData)
            {
                //Process row
                string[] fields = parser.ReadFields();
                _data.Add(new T().SetValues(fields));
            }
        }

        return this;
    }

    public DataStore<T> Save()
    {
        // Save data to file in _storagePath
        using (var writer = new StreamWriter(_storagePath))
        {
            foreach (var item in _data)
            {
                writer.WriteLine(item.GetValues().Aggregate((a, b) => a + "," + b));
            }
        }

        return this;
    }



}