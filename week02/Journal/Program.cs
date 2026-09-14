// Course references:
// https://byui-cse.github.io/cse210-ww-course/week02/design.html
// https://byui-cse.github.io/cse210-ww-course/week02/develop.html

class Program
{
    static void Main(string[] args)
    {
        Journal journal = new Journal();
        PromptGenerator promptGenerator = new PromptGenerator();
        bool running = true;

        while (running)
        {
            Console.WriteLine("Please select one of the following choices:");
            Console.WriteLine("1. Write");
            Console.WriteLine("2. Display");
            Console.WriteLine("3. Load");
            Console.WriteLine("4. Save");
            Console.WriteLine("5. Quit");
            Console.Write("What would you like to do? ");

            string choice = Console.ReadLine();

            if (choice == null)
            {
                Console.WriteLine();
                break;
            }

            switch (choice)
            {
                case "1":
                    string prompt = promptGenerator.GetRandomPrompt();
                    Console.WriteLine(prompt);
                    Console.Write("> ");
                    string response = Console.ReadLine();

                    if (response == null)
                    {
                        Console.WriteLine();
                        running = false;
                        break;
                    }

                    Entry entry = new Entry
                    {
                        _date = DateTime.Now.ToShortDateString(),
                        _promptText = prompt,
                        _entryText = response
                    };

                    journal.AddEntry(entry);
                    break;
                case "2":
                    journal.DisplayAll();
                    break;
                case "3":
                    Console.Write("What is the filename? ");
                    string loadFileName = Console.ReadLine();

                    if (loadFileName == null)
                    {
                        Console.WriteLine();
                        running = false;
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(loadFileName))
                    {
                        Console.WriteLine("A filename is required.");
                    }
                    else
                    {
                        journal.LoadFromFile(loadFileName);
                    }

                    break;
                case "4":
                    Console.Write("What is the filename? ");
                    string saveFileName = Console.ReadLine();

                    if (saveFileName == null)
                    {
                        Console.WriteLine();
                        running = false;
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(saveFileName))
                    {
                        Console.WriteLine("A filename is required.");
                    }
                    else
                    {
                        journal.SaveToFile(saveFileName);
                    }

                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Please enter a number from 1 to 5.");
                    break;
            }

            Console.WriteLine();
        }
    }
}
