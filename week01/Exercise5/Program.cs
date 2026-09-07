using System;

class Program
{
    static void Main(string[] args)
    {
        static void DisplayWelcome()
        {
            Console.WriteLine("Welcome to the Program!");
        }

        static string DisplayPersonalMessage()
        {
            Console.Write("What is your name? ");
            return Console.ReadLine();
        }

        static int PromptUserNumber()
        {
            Console.Write("What is your favorite number: ");
            return int.Parse(Console.ReadLine());
        }
        static int SquareNumber(int number)
        {
            return number * number;
        }

        static void DisplayResult(string userName, int square)
        {
            Console.WriteLine($"{userName}, the square of your number is {square}");
        }

        DisplayWelcome();

        string name = DisplayPersonalMessage();
        int favoriteNumber = PromptUserNumber();

        int squaredNumber = SquareNumber(favoriteNumber);

        DisplayResult(name, squaredNumber);

    }

}