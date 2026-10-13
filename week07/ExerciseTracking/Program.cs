class Program
{
    static void Main(string[] args)
    {
        DateTime date = new DateTime(2022, 11, 3);
        List<Activity> activities = new List<Activity>
        {
            new Running(date, 30, 5.0),
            new Cycling(date, 45, 20.0),
            new Swimming(date, 20, 20)
        };

        foreach (Activity activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}
