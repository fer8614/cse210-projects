using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
class Program
{
    static void Main(string[] args)
    {
        List<int> numbers = new List<int>();
        int number = -1;

        Console.WriteLine("Enter a list of numbers, type 0 when finished.");

        while (number != 0)
        {
            Console.Write("Enter number: ");
            number = int.Parse(Console.ReadLine());

            if (number != 0)
            {
                numbers.Add(number);
            }
        }

        int sum = 0;
        int largest = 0;
        int smallestPositive = int.MaxValue;

        Console.WriteLine("\nThe numbers you entered are:");
        foreach (int item in numbers)
        {
            Console.WriteLine(item);
            sum += item;
            if (item > largest)
            {
                largest = item;
            }

            if (item > 0 && item < smallestPositive)
            {
                smallestPositive = item;
            }

        }
        float average = ((float)sum) / numbers.Count;

        Console.WriteLine($"The total sum is: {sum}");
        Console.WriteLine($"the average is: {average}");
        Console.WriteLine($"the largest number is: {largest}");

        if (smallestPositive != int.MaxValue)
        {
            Console.WriteLine($"The smallest positive number is: {smallestPositive}");
        }
        else
        {
            Console.WriteLine("No positive numbers were entered.");
        }

        numbers.Sort();

        Console.WriteLine("The sorted list is: ");
        foreach (int item in numbers)
        {
            Console.WriteLine(item);
        }

    }
}