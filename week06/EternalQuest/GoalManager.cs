using System.Globalization;

public class GoalManager
{
    private List<Goal> _goals = new List<Goal>();
    // A long score permits many int rewards without wrapping at int.MaxValue.
    private long _score;
    private bool _endOfInput;

    public long GetScore() => _score;
    public long GetLevel() => _score / 1000 + 1;
    public IReadOnlyList<Goal> GetGoals() => _goals.AsReadOnly();

    public void AddGoal(Goal goal)
    {
        ArgumentNullException.ThrowIfNull(goal);
        _goals.Add(goal);
    }

    public void DisplayPlayerInfo()
    {
        long level = GetLevel();
        string title = level >= 10 ? "Champion" : level >= 5 ? "Achiever" : level >= 2 ? "Explorer" : "Beginner";
        Console.WriteLine($"Score: {_score} | Level: {level} | Title: {title}");
    }

    public void Start()
    {
        _endOfInput = false;
        while (!_endOfInput)
        {
            DisplayPlayerInfo();
            Console.WriteLine("1. Create goal\n2. List goals\n3. Save goals\n4. Load goals\n5. Record event\n6. Quit");
            int? choice = ReadNumber("Choose an option: ", 1, 6);
            if (!choice.HasValue || choice == 6)
                return;
            switch (choice.Value)
            {
                case 1: CreateGoal(); break;
                case 2: ListGoalDetails(); break;
                case 3: SaveGoals(); break;
                case 4: LoadGoals(); break;
                case 5: RecordEvent(); break;
            }
        }
    }

    private string ReadText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = Console.ReadLine();
            if (value == null)
            {
                _endOfInput = true;
                return null;
            }
            if (!string.IsNullOrWhiteSpace(value))
                return value;
            Console.WriteLine("Please enter nonempty text.");
        }
    }

    private int? ReadNumber(string prompt, int minimum, int maximum = int.MaxValue)
    {
        while (true)
        {
            string value = ReadText(prompt);
            if (value == null)
                return null;
            if (int.TryParse(value, out int number) && number >= minimum && number <= maximum)
                return number;
            Console.WriteLine($"Enter an integer from {minimum} to {maximum}.");
        }
    }

    public void CreateGoal()
    {
        Console.WriteLine("1. Simple\n2. Eternal\n3. Checklist");
        int? type = ReadNumber("Goal type: ", 1, 3);
        if (!type.HasValue) return;
        string name = ReadText("Name: ");
        if (name == null) return;
        string description = ReadText("Description: ");
        if (description == null) return;
        int? points = ReadNumber("Points: ", 0);
        if (!points.HasValue) return;
        if (type == 1)
            AddGoal(new SimpleGoal(name, description, points.Value));
        else if (type == 2)
            AddGoal(new EternalGoal(name, description, points.Value));
        else
        {
            int? target = ReadNumber("Target: ", 1);
            if (!target.HasValue) return;
            int? bonus = ReadNumber("Bonus: ", 0, int.MaxValue - points.Value);
            if (!bonus.HasValue) return;
            AddGoal(new ChecklistGoal(name, description, points.Value, target.Value, bonus.Value));
        }
    }

    public void ListGoalNames()
    {
        for (int i = 0; i < _goals.Count; i++)
        {
            string fields = _goals[i].GetStringRepresentation().Split(':')[1];
            Console.WriteLine($"{i + 1}. {Uri.UnescapeDataString(fields.Split(',')[0])}");
        }
    }

    public void ListGoalDetails()
    {
        if (_goals.Count == 0)
            Console.WriteLine("No goals yet.");
        for (int i = 0; i < _goals.Count; i++)
            Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
    }

    public int RecordEvent(int index)
    {
        if (index < 0 || index >= _goals.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        // Preview on a copy before mutating progress, so even long overflow is atomic.
        Goal preview = ParseGoal(_goals[index].GetStringRepresentation());
        int reward = preview.RecordEvent();
        long newScore = checked(_score + reward);
        _goals[index].RecordEvent();
        _score = newScore;
        return reward;
    }

    public void RecordEvent()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals yet.");
            return;
        }
        ListGoalNames();
        int? number = ReadNumber("Goal number: ", 1, _goals.Count);
        if (!number.HasValue) return;
        try { Console.WriteLine($"You earned {RecordEvent(number.Value - 1)} points."); }
        catch (OverflowException) { Console.WriteLine("Score limit reached; event was not recorded."); }
    }

    public void SaveGoals(string filename)
    {
        using StreamWriter writer = new StreamWriter(filename);
        writer.WriteLine(_score.ToString(CultureInfo.InvariantCulture));
        foreach (Goal goal in _goals)
            writer.WriteLine(goal.GetStringRepresentation());
    }

    public void LoadGoals(string filename)
    {
        using StreamReader reader = new StreamReader(filename);
        if (!long.TryParse(reader.ReadLine(), NumberStyles.Integer, CultureInfo.InvariantCulture, out long score) || score < 0)
            throw new FormatException("Invalid score.");
        List<Goal> goals = new List<Goal>();
        string line;
        while ((line = reader.ReadLine()) != null)
            goals.Add(ParseGoal(line));
        _goals = goals;
        _score = score;
    }

    private static Goal ParseGoal(string line)
    {
        int separator = line.IndexOf(':');
        if (separator < 0) throw new FormatException("Missing goal type.");
        string type = line.Substring(0, separator);
        string[] fields = line.Substring(separator + 1).Split(',');
        int expected = type == "SimpleGoal" ? 4 : type == "EternalGoal" ? 3 : type == "ChecklistGoal" ? 6 : 0;
        if (expected == 0 || fields.Length != expected)
            throw new FormatException("Unknown goal type or wrong field count.");
        string name = Uri.UnescapeDataString(fields[0]);
        string description = Uri.UnescapeDataString(fields[1]);
        int points = int.Parse(fields[2], NumberStyles.Integer, CultureInfo.InvariantCulture);
        try
        {
            if (type == "SimpleGoal")
                return new SimpleGoal(name, description, points, bool.Parse(fields[3]));
            if (type == "EternalGoal")
                return new EternalGoal(name, description, points);
            int target = int.Parse(fields[3], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int bonus = int.Parse(fields[4], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int completed = int.Parse(fields[5], NumberStyles.Integer, CultureInfo.InvariantCulture);
            return new ChecklistGoal(name, description, points, target, bonus, completed);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            throw new FormatException("Invalid goal bounds.", ex);
        }
    }

    public void SaveGoals() => PromptForFile(true);
    public void LoadGoals() => PromptForFile(false);

    private void PromptForFile(bool saving)
    {
        string filename = ReadText("Filename: ");
        if (filename == null) return;
        try
        {
            if (saving) SaveGoals(filename);
            else LoadGoals(filename);
            Console.WriteLine(saving ? "Goals saved." : "Goals loaded.");
        }
        catch (IOException ex) { Console.WriteLine($"File error: {ex.Message}"); }
        catch (UnauthorizedAccessException ex) { Console.WriteLine($"Access error: {ex.Message}"); }
        catch (FormatException ex) { Console.WriteLine($"Invalid file: {ex.Message}"); }
        catch (OverflowException) { Console.WriteLine("Invalid file: number is too large."); }
        catch (ArgumentException ex) { Console.WriteLine($"Invalid filename: {ex.Message}"); }
    }
}
