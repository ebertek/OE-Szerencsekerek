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
            Wheel wheel = new Wheel();
            // System.Collections.Generic.List<Person> players = new System.Collections.Generic.List<Person>();
            int currentPlayer = 0;
            Console.Write("Hány játékos játszik? ");
            int playerCount = int.Parse(Console.ReadLine());
            Person[] players = new Person[playerCount];
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Person();
            }
            //board.Draw();
            while (!board.gameOver)
            {
                int Spun = wheel.Spin();
                Console.Clear();
                board.Draw();
                for (int i = 0; i < players.Length; i++)
                {
                    Console.WriteLine(i + 1 + ". játékos: " + String.Format(CI, "{0:C0}", players[i].Winnings));
                }
                Console.Write(currentPlayer+1 + ". játékos, adj meg egy betűt " + String.Format(CI, "{0:C0}", Spun) + "-ért: ");
                players[currentPlayer].Winnings += Spun * board.Try(Console.ReadKey().KeyChar);
                currentPlayer++;
                if (currentPlayer == players.Length)
                {
                    currentPlayer = 0;
                }
            }
            Console.Clear();
            board.Draw();
            int winner = 0;
            for (int i = 0; i < players.Length; i++)
            {
                Console.WriteLine(i + 1 + ". játékos: " + String.Format(CI, "{0:C0}", players[i].Winnings));
                if (players[i].Winnings > players[winner].Winnings)
                {
                    winner = i;
                }
            }
            Console.WriteLine("Gratulálok, " + (winner + 1) + ". játékos, nyertél!");
            // Console.ReadKey();
        }
    }
}
