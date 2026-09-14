public class Journal
{
    private const string Separator = "~|~";

    public List<Entry> _entries = new List<Entry>();

    public void AddEntry(Entry newEntry)
    {
        _entries.Add(newEntry);
    }

    public void DisplayAll()
    {
        foreach (Entry entry in _entries)
        {
            entry.Display();
            Console.WriteLine();
        }
    }

    public void SaveToFile(string fileName)
    {
        try
        {
            using StreamWriter outputFile = new StreamWriter(fileName);

            foreach (Entry entry in _entries)
            {
                outputFile.WriteLine($"{entry._date}{Separator}{entry._promptText}{Separator}{entry._entryText}");
            }

            Console.WriteLine("Journal saved successfully.");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Unable to save the journal: {exception.Message}");
        }
    }

    public void LoadFromFile(string fileName)
    {
        try
        {
            string[] lines = File.ReadAllLines(fileName);
            List<Entry> loadedEntries = new List<Entry>();

            foreach (string line in lines)
            {
                string[] parts = line.Split(Separator);

                if (parts.Length != 3)
                {
                    throw new FormatException("The file contains an invalid journal entry.");
                }

                Entry entry = new Entry
                {
                    _date = parts[0],
                    _promptText = parts[1],
                    _entryText = parts[2]
                };

                loadedEntries.Add(entry);
            }

            _entries = loadedEntries;
            Console.WriteLine("Journal loaded successfully.");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Unable to load the journal: {exception.Message}");
        }
    }
}
