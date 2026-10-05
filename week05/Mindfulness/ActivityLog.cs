public class ActivityLog
{
    private const string Separator = "|";

    private string _fileName;
    private Dictionary<string, int> _timesCompleted;
    private Dictionary<string, int> _secondsCompleted;

    public ActivityLog(string fileName)
    {
        _fileName = fileName;
        _timesCompleted = new Dictionary<string, int>();
        _secondsCompleted = new Dictionary<string, int>();
    }

    public void Record(Activity activity)
    {
        string name = activity.GetName();

        _timesCompleted[name] = _timesCompleted.GetValueOrDefault(name) + 1;
        _secondsCompleted[name] = _secondsCompleted.GetValueOrDefault(name) + activity.GetDuration();
    }

    public void Display()
    {
        Console.Clear();
        Console.WriteLine("Activity Log");
        Console.WriteLine();

        if (_timesCompleted.Count == 0)
        {
            Console.WriteLine("You have not completed any activities yet.");
            return;
        }

        foreach (string name in _timesCompleted.Keys)
        {
            Console.WriteLine($"{name}: {_timesCompleted[name]} time(s), {_secondsCompleted[name]} seconds total");
        }
    }

    public void Load()
    {
        if (!File.Exists(_fileName))
        {
            return;
        }

        foreach (string line in File.ReadAllLines(_fileName))
        {
            string[] parts = line.Split(Separator);

            if (parts.Length == 3
                && int.TryParse(parts[1], out int times)
                && int.TryParse(parts[2], out int seconds))
            {
                _timesCompleted[parts[0]] = times;
                _secondsCompleted[parts[0]] = seconds;
            }
        }
    }

    public void Save()
    {
        using StreamWriter outputFile = new StreamWriter(_fileName);

        foreach (string name in _timesCompleted.Keys)
        {
            outputFile.WriteLine($"{name}{Separator}{_timesCompleted[name]}{Separator}{_secondsCompleted[name]}");
        }
    }
}
