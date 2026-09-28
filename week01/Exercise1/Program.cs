using System;
using System.ComponentModel;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("What is your first name? ");
        string first = Console.ReadLine();

        Console.Write("What is your last name? ");
        string last = Console.ReadLine();

        Console.WriteLine($"Your name is {last}, {first} {last}.");


        int[] notas = { 55, 80, 92, 60, 45 };
        int menor = notas[0];
        int mayor = notas[0];

        foreach (int nota in notas)
        {
            if (nota < menor)
            {
                menor = nota;
            }
            else if(nota > mayor)
            {
                mayor = nota;
            }
        }
        Console.WriteLine($"La nota más baja es {menor}");
        Console.WriteLine($"La nota más alta es {mayor}");

        void MostrarMeta(string lenguaje)
        {
            Console.WriteLine($"Mi meta es aprender {lenguaje}");
        }
        MostrarMeta("C#");
        MostrarMeta("Python");

    }
}