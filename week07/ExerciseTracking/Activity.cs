using System.Globalization;

public abstract class Activity
{
    private DateTime _date;
    private int _minutes;

    protected Activity(DateTime date, int minutes)
    {
        _date = date;
        _minutes = minutes;
    }

    protected int GetMinutes() => _minutes;

    public abstract double GetDistance();
    public abstract double GetSpeed();
    public abstract double GetPace();

    protected abstract string GetActivityName();

    public string GetSummary()
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            "{0} {1} ({2} min): Distance: {3:F1} km, Speed: {4:F1} kph, Pace: {5:F1} min per km",
            _date.ToString("dd MMM yyyy", CultureInfo.InvariantCulture),
            GetActivityName(), _minutes, GetDistance(), GetSpeed(), GetPace());
    }
}
