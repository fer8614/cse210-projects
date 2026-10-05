public class Activity
{
    private string _name;
    private string _description;
    private int _duration;
    private Random _random;

    public Activity(string name, string description)
    {
        _name = name;
        _description = description;
        _duration = 0;
        _random = new Random();
    }

    public string GetName()
    {
        return _name;
    }

    public int GetDuration()
    {
        return _duration;
    }

    public void DisplayStartingMessage()
    {
        Console.Clear();
        Console.WriteLine($"Welcome to the {_name} Activity.");
        Console.WriteLine();
        Console.WriteLine(_description);
        Console.WriteLine();

        _duration = ReadDuration();

        Console.Clear();
        Console.WriteLine("Get ready...");
        ShowSpinner(3);
        Console.WriteLine();
    }

    public void DisplayEndingMessage()
    {
        Console.WriteLine();
        Console.WriteLine("Well done!!");
        ShowSpinner(3);
        Console.WriteLine();
        Console.WriteLine($"You have completed another {_duration} seconds of the {_name} Activity.");
        ShowSpinner(3);
    }

    public void ShowSpinner(int seconds)
    {
        string[] frames = { "|", "/", "-", "\\" };
        DateTime endTime = DateTime.Now.AddSeconds(seconds);
        int frameIndex = 0;

        while (DateTime.Now < endTime)
        {
            Console.Write(frames[frameIndex]);
            Thread.Sleep(250);
            Console.Write("\b \b");
            frameIndex = (frameIndex + 1) % frames.Length;
        }
    }

    public void ShowCountDown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            string number = i.ToString();
            Console.Write(number);
            Thread.Sleep(1000);
            EraseText(number.Length);
        }
    }

    protected string GetRandomItem(List<string> items, List<string> unusedItems)
    {
        if (unusedItems.Count == 0)
        {
            unusedItems.AddRange(items);
        }

        int index = _random.Next(unusedItems.Count);
        string item = unusedItems[index];
        unusedItems.RemoveAt(index);
        return item;
    }

    private int ReadDuration()
    {
        while (true)
        {
            Console.Write("How long, in seconds, would you like for your session? ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int seconds) && seconds > 0)
            {
                return seconds;
            }

            Console.WriteLine("Please enter a whole number greater than zero.");
        }
    }

    private void EraseText(int length)
    {
        Console.Write(new string('\b', length));
        Console.Write(new string(' ', length));
        Console.Write(new string('\b', length));
    }
}
