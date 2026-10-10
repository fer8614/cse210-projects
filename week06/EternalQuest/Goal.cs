using System.Globalization;

public abstract class Goal
{
    private string _shortName;
    private string _description;
    private int _points;

    protected Goal(string name, string description, int points)
    {
        if (points < 0)
            throw new ArgumentOutOfRangeException(nameof(points), "Points cannot be negative.");
        _shortName = name;
        _description = description;
        _points = points;
    }

    protected string GetShortName() => _shortName;
    protected string GetDescription() => _description;
    protected int GetPoints() => _points;

    // Text is escaped before joining fields so separators remain unambiguous.
    protected string GetCommonRepresentation()
    {
        return $"{Uri.EscapeDataString(GetShortName())},{Uri.EscapeDataString(GetDescription())},{GetPoints().ToString(CultureInfo.InvariantCulture)}";
    }

    public abstract int RecordEvent();
    public abstract bool IsComplete();

    public virtual string GetDetailsString()
    {
        string checkbox = IsComplete() ? "[X]" : "[ ]";
        return $"{checkbox} {_shortName} ({_description})";
    }

    public abstract string GetStringRepresentation();
}
