using System.Globalization;

public class ChecklistGoal : Goal
{
    private int _target;
    private int _bonus;
    private int _amountCompleted;

    public ChecklistGoal(string name, string description, int points, int target, int bonus, int amountCompleted = 0)
        : base(name, description, points)
    {
        if (target <= 0)
            throw new ArgumentOutOfRangeException(nameof(target), "Target must be positive.");
        if (bonus < 0)
            throw new ArgumentOutOfRangeException(nameof(bonus), "Bonus cannot be negative.");
        if (bonus > int.MaxValue - points)
            throw new ArgumentOutOfRangeException(nameof(bonus), "Points plus bonus must fit in an integer.");
        if (amountCompleted < 0 || amountCompleted > target)
            throw new ArgumentOutOfRangeException(nameof(amountCompleted), "Completed count must be between zero and target.");
        _target = target;
        _bonus = bonus;
        _amountCompleted = amountCompleted;
    }

    public override int RecordEvent()
    {
        if (IsComplete())
            return 0;
        _amountCompleted++;
        return IsComplete() ? GetPoints() + _bonus : GetPoints();
    }

    public override bool IsComplete() => _amountCompleted == _target;

    public override string GetDetailsString()
    {
        return $"{base.GetDetailsString()} -- Currently completed: {_amountCompleted}/{_target}";
    }

    public override string GetStringRepresentation()
    {
        // After common fields: target, bonus, amountCompleted (also documented in checks).
        return $"ChecklistGoal:{GetCommonRepresentation()},{_target.ToString(CultureInfo.InvariantCulture)},{_bonus.ToString(CultureInfo.InvariantCulture)},{_amountCompleted.ToString(CultureInfo.InvariantCulture)}";
    }
}
