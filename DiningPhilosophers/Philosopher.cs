using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    class Philosopher
    {

        public string Name { get; set; }
        public PhilosopherState CurrentState { get; set; }
        public Chopstick LeftChopstick { get; set; }
        public Chopstick RightChopstick { get; set; }

        private const int PHILOSOPHER_COUNT = 5; //As there are 5 of them in Dining Philosopher Question
        private const int TIMES_TO_EAT = 5;
        public int timesEaten = 0;

        private List<Philosopher> philosophers;
        private readonly int index;

        public Philosopher(List<Philosopher> philosophers, int index)
        {
            this.philosophers = philosophers;
            this.index = index;
            this.Name = string.Format("Philosopher {0}", index);
            this.CurrentState = PhilosopherState.Thinking;
        }

        public Philosopher LeftPhilosopher
        {
            get
            {
                if (index == 0)
                    return philosophers[philosophers.Count - 1];
                else
                    return philosophers[index - 1];
            }
        }

        public Philosopher RightPhilosopher
        {
            get
            {
                if (index == philosophers.Count - 1)
                    return philosophers[0];
                else
                    return philosophers[index + 1];
            }
        }

        public static List<Philosopher> InitializeDiningPhilosophers()
        {
            // Construct philosophers
            var philosophers = new List<Philosopher>(PHILOSOPHER_COUNT);
            for (int i = 0; i < PHILOSOPHER_COUNT; i++)
            {
                philosophers.Add(new Philosopher(philosophers, i));
            }

            // Assign chopsticks to each philosopher
            foreach (var philosopher in philosophers)
            {
                // Assign left chopstick
                philosopher.LeftChopstick = philosopher.LeftPhilosopher.RightChopstick ?? new Chopstick();

                // Assign right chopstick
                philosopher.RightChopstick = philosopher.RightPhilosopher.LeftChopstick ?? new Chopstick();
            }

            return philosophers;
        }

        /// <summary>
        /// If a philosopher acquire both chopsticks, he starts eating, but only 1 time.
        /// He/she only eats 1 time and wait for other turns to complete his/her entire meal.
        /// </summary>
        public void EatAll()
        {
            while (timesEaten < TIMES_TO_EAT)
            {
                this.Think();
                if (this.PickUp())
                {
                    this.Eat();

                    this.PutDownLeft();
                    this.PutDownRight();
                }
            }
        }

        /// <summary>
        /// First, a philosopher picks up his/her left chopstick. Then, pick up the right one.
        /// If he/she doesn't aquire enough chopsticks to start eating, he/she puts everything down.
        /// </summary>
        /// <returns></returns>
        private bool PickUp()
        {
            if (System.Threading.Monitor.TryEnter(this.LeftChopstick))
            {
                Console.WriteLine(this.Name + " picks up left chopstick.");
                if (System.Threading.Monitor.TryEnter(this.RightChopstick))
                {
                    Console.WriteLine(this.Name + " picks up right chopstick.");
                    return true;
                }
                else
                {
                    this.PutDownLeft();
                }
            }
            return false;
        }

        private void Eat()
        {
            this.CurrentState = PhilosopherState.Eating;
            timesEaten++;
            Console.WriteLine(this.Name + " eats.");
        }

        private void PutDownLeft()
        {
            System.Threading.Monitor.Exit(this.LeftChopstick);
            Console.WriteLine(this.Name + " puts down left chopstick.");
        }

        private void PutDownRight()
        {
            System.Threading.Monitor.Exit(this.RightChopstick);
            Console.WriteLine(this.Name + " puts down right chopstick.");
        }


        private void Think()
        {
            this.CurrentState = PhilosopherState.Thinking;
        }
    }

}
