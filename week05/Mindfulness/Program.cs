using System;

// Exceeding requirements:
// 1. Prompts and questions are never repeated until every one of them has
//    been used at least once in the session (shared GetRandomItem helper in
//    the Activity base class).
// 2. An activity log keeps track of how many times each activity was done
//    and the total seconds spent in it. It can be viewed from the menu.
// 3. The log is saved to a file when the user quits and loaded again the
//    next time the program starts, so the totals carry over between sessions.
// 4. The duration input is validated, so the program asks again instead of
//    crashing when the user types something that is not a positive number.
class Program
{
    static void Main(string[] args)
    {
        string logFile = Path.Combine(AppContext.BaseDirectory, "mindfulness_log.txt");
        ActivityLog log = new ActivityLog(logFile);
        log.Load();

        BreathingActivity breathing = new BreathingActivity();
        ReflectingActivity reflecting = new ReflectingActivity();
        ListingActivity listing = new ListingActivity();

        string choice = "";

        while (choice != "5")
        {
            Console.Clear();
            Console.WriteLine("Menu Options:");
            Console.WriteLine("  1. Start breathing activity");
            Console.WriteLine("  2. Start reflecting activity");
            Console.WriteLine("  3. Start listing activity");
            Console.WriteLine("  4. View activity log");
            Console.WriteLine("  5. Quit");
            Console.Write("Select a choice from the menu: ");
            choice = Console.ReadLine() ?? "5";

            if (choice == "1")
            {
                breathing.Run();
                log.Record(breathing);
            }
            else if (choice == "2")
            {
                reflecting.Run();
                log.Record(reflecting);
            }
            else if (choice == "3")
            {
                listing.Run();
                log.Record(listing);
            }
            else if (choice == "4")
            {
                log.Display();
                Console.WriteLine();
                Console.Write("Press enter to return to the menu.");
                Console.ReadLine();
            }
        }

        log.Save();
        Console.WriteLine("Goodbye!");
    }
}
