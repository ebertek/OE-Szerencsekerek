using System;

namespace Szerencsekerek
{
    class Wheel
    {
        private readonly int[] layout;
        public Wheel(int[] layout)
        {
            this.layout = layout;
        }
        public int Spin()
        {
            Random rnd = new Random();
            return layout[rnd.Next(layout.Length)];
        }
    }
    class Board
    {
        private readonly string puzzle;
        private const char mask = '-';
        private readonly bool[] solved;
        private readonly int length;
        private int done;
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
        public int Guess(char letter)
        {
            letter = Char.ToUpper(letter);
            if ("BCDFGHJKLMNPQRSTVWXZ".Contains(letter))
            {
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
                gameOver |= done >= length;
                return Correct;
            } else
            if (Char.IsDigit(letter))
            {
                return -2; // Special functions
            } else
            {
                Console.Beep();
                return -1; // Vowel / unacceptable character
            }
        }
        public bool Guess(string solution)
        {
            if (string.Equals(solution, puzzle, StringComparison.InvariantCultureIgnoreCase)) {
                gameOver = true;
                return true;
            } else
            {
                return false;
            }
        }
    }
    class Player
    {
        private int winnings;
        public int Winnings
        {
            get { return winnings; }
            /* set { winnings = value; } */
        }
        public void Add(int amount)
        {
            winnings += amount;
        }
        public void Reset()
        {
            winnings = 0;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            /* Initialize the Game */
            System.Globalization.CultureInfo CI = new System.Globalization.CultureInfo("hu-HU");
            /* int[] MTV = new int[] { 0, 1700, 5500, 1100, 6000, 1100, 2000, 1100, 1500, 5500, 1300, 4000, 900, 1100, 11000, 1600, 1200, 4000, 1500, 1200, 6000, 1000, 13000 }; */
            int[] husz = { 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000, 15000, 16000, 17000, 18000, 19000, 20000 };
            Wheel wheel = new Wheel(husz);
            int playerCount = 3;
            /* Console.Write("Hány játékos játszik? ");
            int playerCount = int.Parse(Console.ReadLine()); */
            Player[] players = new Player[playerCount];
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Player();
            }
            int rounds = 3;
            /* Console.Write("Hány kört játsszunk? ");
            int rounds = int.Parse(Console.ReadLine()); */

            string fileName = "kozmondasok.txt";
            if (args.Length != 0)
            {
                fileName = args[0];
            }

            /* Start the Game */
            for (int i = 1; i <= rounds; i++)
            {
                Board board = new Board(System.IO.File.ReadAllLines(fileName));
                int currentPlayer = 0;
                int winner = -1;
                while (!board.gameOver)
                {
                    /* Spin the Wheel */
                    int spun = wheel.Spin();
                    while (spun == 0)
                    {
                        Console.Beep();
                        spun = wheel.Spin();
                        currentPlayer++;
                        if (currentPlayer == players.Length)
                        {
                            currentPlayer = 0;
                        }
                    }

                    /* Draw the Puzzle Board and Standings */
                    Console.Clear();
                    board.Draw();
                    for (int j = 0; j < players.Length; j++)
                    {
                        Console.WriteLine(j + 1 + ". játékos: " + String.Format(CI, "{0:C0}", players[j].Winnings));
                    }

                    /* Get user input */
                    int correct = -1;
                    Console.WriteLine();
                    while (correct == -1)
                    {
                        ClearCurrentLine();
                        Console.Write(currentPlayer + 1 + ". játékos, adj meg egy betűt " + String.Format(CI, "{0:C0}", spun) + "-ért: ");
                        correct = board.Guess(Console.ReadKey().KeyChar);
                    }
                    if (correct > 0)
                    {
                        players[currentPlayer].Add(spun * correct);
                    }
                    else
                    if (correct == 0)
                    {
                        currentPlayer++;
                        if (currentPlayer == players.Length)
                        {
                            currentPlayer = 0;
                        }
                    }
                    else
                    {
                        ClearCurrentLine();
                        Console.Write(currentPlayer + 1 + ". játékos, add meg a megoldást " + String.Format(CI, "{0:C0}", players[currentPlayer].Winnings) + "-ért: ");
                        if (!board.Guess(Console.ReadLine()))
                        {
                            currentPlayer++;
                            if (currentPlayer == players.Length)
                            {
                                currentPlayer = 0;
                            }
                        } else
                        {
                            winner = currentPlayer;
                        }
                    }
                } // while
                /* Only the winner of the round gets to keep their points */
                for (int j = 0; j < players.Length; j++)
                {
                    if (j != winner)
                    {
                        players[j].Reset();
                    }
                }
            }

            /* Final Results */
            Console.Clear();
            int finalWinner = 0;
            for (int i = 0; i < players.Length; i++)
            {
                Console.WriteLine(i + 1 + ". játékos: " + String.Format(CI, "{0:C0}", players[i].Winnings));
                if (players[i].Winnings > players[finalWinner].Winnings)
                {
                    finalWinner = i;
                }
            }
            Console.WriteLine();
            Console.WriteLine("Gratulálok, " + (finalWinner + 1) + ". játékos, nyertél!");
            // Console.ReadKey();
        }
        static void ClearCurrentLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop);
        }
    }
}
