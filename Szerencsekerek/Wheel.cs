using System;

namespace Szerencsekerek
{
    class Wheel
    {
        private readonly int[] layout; // wheel layout
        private static readonly Random rnd = new Random(); // rng for spinning the wheel

        public Wheel(int[] layout)
        {
            this.layout = layout;
        }
        public int Spin()
        {
            return layout[rnd.Next(layout.Length)];
        }
    }
}
