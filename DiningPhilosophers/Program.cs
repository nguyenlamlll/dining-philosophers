// Framework used: .NET Framework 4
// Compiler used: v4.0.30319.1
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace DiningPhilosophers
{
    class Program
    {
        private const int PHILOSOPHER_COUNT = 5;

        static void Main(string[] args)
        {
            // Construct philosophers and chopsticks
            var philosophers = Philosopher.InitializeDiningPhilosophers();

            Console.WriteLine("Dinner is starting!\n");

            // Spawn threads for each philosopher's eating cycle
            var philosopherThreads = new List<Thread>();
            foreach (var philosopher in philosophers)
            {
                var philosopherThread = new Thread(new ThreadStart(philosopher.EatAll));
                philosopherThreads.Add(philosopherThread);
                philosopherThread.Start();
            }

            // Wait for all philosopher's to finish eating
            foreach (var thread in philosopherThreads)
            {
                thread.Join();
            }

            // Done
            Console.WriteLine("Dinner is over!\n");

            foreach (var philosopher in philosophers)
            {
                string times = philosopher.timesEaten.ToString();
                Console.WriteLine(philosopher.Name + " Ate " + times + " times.");
            }

            Console.ReadLine();
        }

    }
}