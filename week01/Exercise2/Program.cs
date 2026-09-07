using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("What is your grade percentage?");
        string percentage = Console.ReadLine();
        int number = int.Parse(percentage);

        if (number >= 90)
        {
            if (number >= 93)
            {
                Console.WriteLine($"Your grade is A");
            }
            else
            {
                Console.WriteLine($"Your grade is A-");

            }
        }
        else if (number >= 80)
        {
            if (number >= 87)
            {
                Console.WriteLine("Your grade is B+");

            }
            else if (number <= 82)
            {
                Console.WriteLine("Your grade is B-");

            }
            else
            {
                Console.WriteLine("Your grade is B");
            }
        }
        else if (number >= 70)
        {
            if (number >= 77)
            {
                Console.WriteLine("Your grade is C+");

            }
            else if (number <= 72)
            {
                Console.WriteLine("Your grade is C-");

            }
            else
            {
                Console.WriteLine("Your grade is C");
            }
        }
        else if (number >= 60)
        {
            if (number >= 67)
            {
                Console.WriteLine("Your grade is D+");

            }
            else if (number <= 62)
            {
                Console.WriteLine("Your grade is D-");

            }
            else
            {
                Console.WriteLine("Your grade is D");
            }
        }
        else if (number < 60)
        {
            Console.WriteLine("Your grade is F");
        }
        else
        {
            Console.WriteLine("You don't have grade");
        }
        if (number >= 70)
        {
            Console.WriteLine("You passed!");
        }
        else
        {
            Console.WriteLine("Better luck next time!");
        }

    }

}