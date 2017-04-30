using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    class Chopstick
    {
        private static int count = 0;
        public string Name { get; set; }
        public Chopstick()
        {
            Name = "Chopstick " + (++count);
        }
    }
}
