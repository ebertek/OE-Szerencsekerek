using System;

namespace Szerencsekerek
{
    class Wheel
    {
        private readonly int[] layout = new int[] { 0, 1700, 5500, 1100, 6000, 1100, 2000, 1100, 1500, 5500, 1300, 4000, 900, 1100, 11000, 1600, 1200, 4000, 1500, 1200, 6000, 1000, 13000 };
        public int Spin()
        {
            Random rnd = new Random();
            int idx = rnd.Next(layout.Length);
            return layout[idx];
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Wheel Wheel = new Wheel();
            int Spun = Wheel.Spin();
            Console.WriteLine(Spun);
            Console.ReadKey();
        }
    }
}
