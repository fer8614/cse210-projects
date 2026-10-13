public class Running : Activity
{
    private double _distanceKm;

    public Running(DateTime date, int minutes, double distanceKm) : base(date, minutes)
    {
        _distanceKm = distanceKm;
    }

    public override double GetDistance() => _distanceKm;
    public override double GetSpeed() => GetDistance() / GetMinutes() * 60;
    public override double GetPace() => GetMinutes() / GetDistance();

    protected override string GetActivityName() => "Running";
}
