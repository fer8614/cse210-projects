using System;
using System.ComponentModel;
using System.Net;

class Program
{
    static void Main(string[] args)
    {
        Random randomNumber = new Random();

        string playAgain;


        do
        {
            int magicNumber = randomNumber.Next(1, 101);
            int guessNumber = -1;
            int attempts = 0;
            Console.WriteLine("what is your guess?");

            while (guessNumber != magicNumber)
            {
                attempts++;

                guessNumber = int.Parse(Console.ReadLine());


                if (guessNumber < magicNumber)
                {
                    Console.WriteLine("Higher");
                }
                else if (guessNumber > magicNumber)
                {
                    Console.WriteLine("Lower");
                }
            }
            Console.WriteLine($"congratulations {guessNumber} is the Magic Number, and you guessed them in {attempts} tries");
            Console.Write("\nDo you want to play again? (yes/no): ");
            playAgain = Console.ReadLine();

        } while (playAgain == "yes" || playAgain == "y");

        Console.WriteLine("\nThanks for playing! Goodbye.");


    }

}
