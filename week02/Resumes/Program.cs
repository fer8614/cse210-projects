using System;

class Program
{
    static void Main(string[] args)
    {
        // 1. Crear el primer trabajo
        Job job1 = new Job();
        job1._jobTitle = "Software Engineer";
        job1._company = "Microsoft";
        job1._startYear = 2019;
        job1._endYear = 2022;

        // 2. Crear el segundo trabajo
        Job job2 = new Job();
        job2._jobTitle = "Manager";
        job2._company = "Apple";
        job2._startYear = 2022;
        job2._endYear = 2023;

        // 3. Crear el currículum y asignar el nombre
        Resume myResume = new Resume();
        myResume._name = "Allison Rose";

        // 4. Agregar los trabajos a la lista del currículum
        myResume._jobs.Add(job1);
        myResume._jobs.Add(job2);

        // 5. Mostrar todo el currículum en consola
        myResume.Display();
    }
}