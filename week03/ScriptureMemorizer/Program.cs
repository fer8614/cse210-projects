using System;
using System.IO;

// Exceeded requirements:
// 1. Loads a library of scriptures from scriptures.txt and chooses one at random.
// 2. Only hides words that are not already hidden (stretch challenge).
class Program
{
    static void Main(string[] args)
    {
        Scripture scripture = LoadRandomScripture();

        while (true)
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine();
            Console.WriteLine("Press enter to continue or type 'quit' to finish:");

            string input = Console.ReadLine();
            if (input != null && input.ToLower() == "quit")
            {
                break;
            }

            if (scripture.IsCompletelyHidden())
            {
                break;
            }

            scripture.HideRandomWords(3);
        }
    }

    static Scripture LoadRandomScripture()
    {
        string filePath = FindScripturesFile();
        string[] lines = File.ReadAllLines(filePath);
        Random random = new Random();
        string line = lines[random.Next(lines.Length)];
        string[] parts = line.Split('|');

        Reference reference;
        string text;

        if (parts.Length == 4)
        {
            // book|chapter|verse|text
            reference = new Reference(parts[0], int.Parse(parts[1]), int.Parse(parts[2]));
            text = parts[3];
        }
        else
        {
            // book|chapter|startVerse|endVerse|text
            reference = new Reference(
                parts[0],
                int.Parse(parts[1]),
                int.Parse(parts[2]),
                int.Parse(parts[3]));
            text = parts[4];
        }

        return new Scripture(reference, text);
    }

    static string FindScripturesFile()
    {
        string[] candidates =
        {
            "scriptures.txt",
            Path.Combine(AppContext.BaseDirectory, "scriptures.txt")
        };

        foreach (string candidate in candidates)
        {
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        throw new FileNotFoundException("Could not find scriptures.txt.");
    }
}
