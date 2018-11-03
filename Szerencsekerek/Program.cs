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
        private bool[] solved;
        private int length, done;
        public bool gameOver;
        public Board(string puzzle)
        {
            this.puzzle = puzzle.ToUpper();
            length = puzzle.Length;
            solved = new bool[length];
            Normalize();
        }
        public Board(string[] puzzles)
        {
            Random rnd = new Random();
            puzzle = puzzles[rnd.Next(puzzles.Length)].ToUpper();
            length = puzzle.Length;
            solved = new bool[length];
            Normalize();
        }
        private void Normalize()
        {
            for (int i = 0; i < length; i++)
            {
                if (!Char.IsLetter(puzzle[i]))
                {
                    solved[i] = true;
                    done++;
                }
            }
        }
        public void Draw()
        {
            for (int i = 0; i < length; i++)
            {
                if (solved[i])
                {
                    Console.Write(puzzle[i]);
                }
                else
                {
                    Console.Write(mask);
                }
            }
            Console.Write('\n');
        }
        public int Try(char letter)
        {
            letter = Char.ToUpper(letter);
            int Correct = 0;
            for (int i = 0; i < length; i++)
            {
                if (!solved[i] && puzzle[i] == letter)
                {
                    solved[i] = true;
                    Correct++;
                }
            }
            done += Correct;
            if (done >= length) {
                gameOver = true;
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
            System.Globalization.CultureInfo CI = new System.Globalization.CultureInfo("hu-HU");
            Board board = new Board(System.IO.File.ReadAllLines("kozmondasok.txt"));
            board.Draw();
            Wheel wheel = new Wheel();
            Person player1 = new Person();
            while (!board.gameOver)
            {
                int Spun = wheel.Spin();
                Console.Write("Adj meg egy betűt " + String.Format(CI, "{0:C0}", Spun) + "-ért: ");
                player1.Winnings += Spun * board.Try(Console.ReadKey().KeyChar);
                Console.Clear();
                board.Draw();
            }
            Console.WriteLine("Gratulálok, nyertél! Nyereményed: " + String.Format(CI, "{0:C0}", player1.Winnings));
            // Console.ReadKey();
        }
    }
}
