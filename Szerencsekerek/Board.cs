using System;

namespace Szerencsekerek
{
    class Board
    {
        readonly string puzzle; // the actual puzzle to solve
        readonly int length; // length of the puzzle
        const char mask = '-'; // masking character
        // const string mask = "💩"; // UTF-32 characters don't fit in a char
        readonly bool[] solved; // true/false for every character in the puzzle
        static readonly Random rnd = new Random(); // rng for selecting a line from a list of puzzles
        int done; // number of characters in the puzzle already solved
        public bool GameOver { get; private set; } // true if the puzzle is solved

        public Board(ref string[] puzzles)
        {
            int i;
            do
            {
                if (Array.TrueForAll(puzzles, String.IsNullOrWhiteSpace))
                {
                    Console.WriteLine("Hiba: nincs elég megoldandó feladvány a bemeneti fájlban.");
                    Environment.Exit(-1);
                }
                i = rnd.Next(puzzles.Length);
                puzzle = puzzles[i].ToUpper();
                puzzles[i] = ""; // a line shouldn't come up more than once per game
                done = 0;
                length = puzzle.Length;
                solved = new bool[length];
                Normalize();
            } while (String.IsNullOrWhiteSpace(puzzle) || done == length);
        }
        void Normalize() // special characters in the puzzle (like ,) should be shown (="solved") by default
        {
            for (int i = 0; i < length; i++)
            {
                if (!Char.IsLetter(puzzle[i]))
                {
                    solved[i] = true;
                    done++;
                }
            }
            GameOver = done >= length;
        }
        public void Draw() // draw the current puzzle board with the unsolved letters masked
        {
            for (int i = 0; i < length; i++)
            {
                if (solved[i])
                {
                    Console.Write(puzzle[i]);
                } else
                {
                    Console.Write(mask);
                }
            }
            Console.Write(Environment.NewLine);
        }
        public int Guess(char letter, bool anyCharacter)
        {
            letter = Char.ToUpper(letter);
            if (anyCharacter || "BCDFGHJKLMNPQRSTVWXZ".Contains(letter.ToString())) //char.ToString() is needed in .NET Framework 4.6.1, not needed in .NET Core
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
                GameOver |= done >= length;
                return Correct;
            } else if (Char.IsDigit(letter))
            {
                return -(int)char.GetNumericValue(letter) - 1; // Special functions
            } else if ((ConsoleKey)letter == ConsoleKey.Escape) // Esc exits the app
            {
                Console.WriteLine();
                Environment.Exit(0);
                return -1;
            } else
            {
                Console.Beep();
                return -1; // Vowel / unacceptable character
            }
        }
        public bool Guess(string solution)
        {
            if (string.Equals(solution, puzzle, StringComparison.InvariantCultureIgnoreCase))
            {
                for (int i = 0; i < length; i++)
                {
                    solved[i] = true;
                }
                done = length;
                GameOver = true;
                return true;
            }
            return false;
        }
    }
}
