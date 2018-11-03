using System;

namespace Szerencsekerek
{
    class Wheel
    {
        private readonly int[] layout = new int[] { 0, 1700, 5500, 1100, 6000, 1100, 2000, 1100, 1500, 5500, 1300, 4000, 900, 1100, 11000, 1600, 1200, 4000, 1500, 1200, 6000, 1000, 13000 };
        public int Spin()
        {
            Random rnd = new Random();
            return layout[rnd.Next(layout.Length)];
        }
    }
    class Board
    {
        private string puzzle, solved;
        public Board(string puzzle)
        {
            this.puzzle = puzzle;
            Hide();
        }
        public Board(string[] puzzles)
        {
            Random rnd = new Random();
            puzzle = puzzles[rnd.Next(puzzles.Length)];
            Hide();
        }
        private void Hide()
        {
            for (int i = 0; i < puzzle.Length; i++)
            {
                if (Char.IsLetter(puzzle[i]))
                {
                    solved += "💩";
                } else
                {
                    solved += puzzle[i];
                }
            }
        }
        public void Draw()
        {
            Console.WriteLine(solved);
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            Wheel Wheel = new Wheel();
            Board Board = new Board(System.IO.File.ReadAllLines("kozmondasok.txt"));
            int Spun = Wheel.Spin();
            Console.WriteLine(Spun + " Ft");
            Board.Draw();
            Console.ReadKey();
        }
    }
}
