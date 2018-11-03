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
        private string puzzle;
        private const char mask = '-';
        private System.Text.StringBuilder solved = new System.Text.StringBuilder();
        public Board(string puzzle)
        {
            this.puzzle = puzzle.ToUpper();
            Hide(mask);
        }
        public Board(string[] puzzles)
        {
            Random rnd = new Random();
            puzzle = puzzles[rnd.Next(puzzles.Length)].ToUpper();
            Hide(mask);
        }
        private void Hide(char character)
        {
            for (int i = 0; i < puzzle.Length; i++)
            {
                if (Char.IsLetter(puzzle[i]))
                {
                    solved.Append(character);
                } else
                {
                    solved.Append(puzzle[i]);
                }
            }
        }
        public void Draw()
        {
            Console.WriteLine(solved);
        }
        public int Try(char letter)
        {
            letter = Char.ToUpper(letter);
            int Correct = 0;
            for (int i = 0; i < puzzle.Length; i++)
            {
                if (puzzle[i] != solved[i] && puzzle[i] == letter)
                {
                    solved[i] = puzzle[i];
                    Correct++;
                }
            }
            return Correct;
        }
    }
    class Person
    {
        public int Winnings;
    }
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(System.IO.File.ReadAllLines("kozmondasok.txt"));
            board.Draw();
            Wheel wheel = new Wheel();
            Person player1 = new Person();
            int Spun = wheel.Spin();
            Console.Write("Adj meg egy betűt: ");
            player1.Winnings += Spun * board.Try(Console.ReadKey().KeyChar);
            Console.Write('\n');
            board.Draw();
            Console.ReadKey();
        }
    }
}
