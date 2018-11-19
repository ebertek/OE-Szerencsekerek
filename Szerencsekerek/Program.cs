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
    class Board
    {
        private readonly string puzzle; // the actual puzzle to solve
        private readonly int length; // length of the puzzle
        private const char mask = '-'; // masking character
        // private const string mask = "💩"; // UTF-32 characters don't fit in a char
        private readonly bool[] solved; // true/false for every character in the puzzle
        private static readonly Random rnd = new Random(); // rng for selecting a line from a list of puzzles
        private int done; // number of characters in the puzzle already solved
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
        private void Normalize() // special characters in the puzzle (like ,) should be shown (="solved") by default
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
                }
                else
                {
                    Console.Write(mask);
                }
            }
            Console.Write('\n');
        }
        public int Guess(char letter, bool anyCharacter)
        {
            letter = Char.ToUpper(letter);
            if (anyCharacter || "BCDFGHJKLMNPQRSTVWXZ".Contains(letter))
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
                return -(int)char.GetNumericValue(letter)-1; // Special functions
            } else if ((ConsoleKey)letter == ConsoleKey.Escape) // Esc exits the app
            {
                // TODO: Esc eats the next character
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
            if (string.Equals(solution, puzzle, StringComparison.InvariantCultureIgnoreCase)) {
                for (int i = 0; i < length; i++)
                {
                    solved[i] = true;
                }
                done = length;
                GameOver = true;
                return true;
            } else
            {
                return false;
            }
        }
    }
    class Shop
    {
        private readonly string[] items;
        private readonly int[] prices;

        public Shop()
        {
            items = new string[] { "Természetkalauz", "Képes albumok és lexikonok", "Elin Lux grillsütős gáztűzhely", "Ramses édességek", "Camey ajándékcsomag", "Electrolux ZL620 porszívó", "Aroma kávédaráló", "3 darab 195 perces videokazetta", "Neutralia csomag", "Golf Junior fűnyíró" };
            prices = new int[] { 4630, 13710, 49900, 900, 2500, 14900, 1299, 1080, 3000, 16490 };

        }
        public string Item(int index)
        {
            if (index >= 0 && index < items.Length)
            {
                return items[index];
            } else
            {
                return "";
            }
        }
        public int Price(int index)
        {
            if (index >= 0 && index < prices.Length)
            {
                return prices[index];
            } else
            {
                return 0;
            }
        }
        public void Buy(int index)
        {
            if (index >= 0 && index < items.Length)
            {
                items[index] = "";
                prices[index] = 0;
            }
        }
        public int LowestPrice()
        {
            int min = int.MaxValue;
            for (int i = 0; i < prices.Length; i++)
            {
                if (prices[i] > 0 && prices[i] < min) {
                    min = prices[i];
                }
            }
            return min;
        }
        public int Length()
        {
            return items.Length;
        }
        public override string ToString()
        {
            string ListOfItems = "";
            System.Globalization.CultureInfo CI = new System.Globalization.CultureInfo("hu-HU"); // TODO: Use the same CI everywhere
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != "")
                {
                    ListOfItems += (i + 1) + ") " + items[i] + ", " + String.Format(CI, "{0:C0}", prices[i]) + "\n";
                }
            }
            return ListOfItems;
        }
    }
    class Player
    {
        public int Winnings { get; private set; } // number of points the player has
        public System.Collections.Generic.List<string> Prizes { get; } = new System.Collections.Generic.List<string>(); // prizes bought by the player

        public void Add(int amount) // add points
        {
            Winnings += amount;
        }
        public bool Subtract(int amount) // subtract points if possible
        {
            if (Winnings - amount >= 0)
            {
                Winnings -= amount;
                return true;
            }
            return false;
        }
        public bool Buy(Shop shop, int index) // buy an item from the shop
        {
            int price = shop.Price(index);
            if (price > 0 && Subtract(price))
            {
                Prizes.Add(shop.Item(index));
                shop.Buy(index);
                return true;
            } else
            {
                return false;
            }
        }
        public void Reset() // lose all points
        {
            Winnings = 0;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            /* Initialize the Game */
            System.Globalization.CultureInfo CI = new System.Globalization.CultureInfo("hu-HU");
            // int[] MTV = new int[] { 0, 1700, 5500, 1100, 6000, 1100, 2000, 1100, 1500, 5500, 1300, 4000, 900, 1100, 11000, 1600, 1200, 4000, 1500, 1200, 6000, 1000, 13000 };
            int[] husz = { 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000, 15000, 16000, 17000, 18000, 19000, 20000 };
            Wheel wheel = new Wheel(husz);
            Shop shop = new Shop();
            const int playerCount = 3;
            /* Console.Write("Hány játékos játszik? ");
            int playerCount = int.Parse(Console.ReadLine()); */
            Player[] players = new Player[playerCount];
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Player();
            }
            const int rounds = 3;
            /* Console.Write("Hány kört játsszunk? ");
            int rounds = int.Parse(Console.ReadLine()); */

            string fileName = "kozmondasok.txt";
            if (args.Length != 0)
            {
                fileName = args[0];
            }
            string[] puzzles;
            if (System.IO.File.Exists(fileName))
            {
                puzzles = System.IO.File.ReadAllLines(fileName);
            } else
            {
                puzzles = new string[] { "Nem mindegy, hogy hol szorít a cipő" }; // example from the original assignment
            }

            const int vowelPrice = 5000;

            /* Start the Game */
            for (int i = 1; i <= rounds; i++)
            {
                Board board = new Board(ref puzzles);
                int currentPlayer = 0;
                int winner = -1;
                /* Draw the Puzzle Board and Standings */
                void ReDraw(bool includeShop) // TODO: Find a better place for this function
                {
                    Console.Clear();
                    Console.WriteLine(i + ". kör \t\t 1) Megoldás 2) Magánhangzó vásárlása (" + String.Format(CI, "{0:C0}", vowelPrice) + ")");
                    board.Draw();
                    for (int j = 0; j < players.Length; j++)
                    {
                        Console.WriteLine(SzinesJatekos(j) + ". játékos: " + String.Format(CI, "{0:C0}", players[j].Winnings) + " " + String.Join(", ", players[j].Prizes));
                    }
                    Console.ResetColor();
                    Console.WriteLine();
                    if (includeShop)
                    {
                        Console.WriteLine("A " + SzinesJatekos(winner) + ". játékos vásárolhat a kirakatból:");
                        Console.ResetColor();
                        Console.WriteLine(shop.ToString());
                        Console.WriteLine("0) Tovább");
                    }
                }
                while (!board.GameOver)
                {
                    /* Spin the Wheel */
                    int spun = wheel.Spin();
                    while (spun == 0) // if someone spins 0, they miss their turn
                    {
                        Console.Beep();
                        if (++currentPlayer == players.Length)
                        {
                            currentPlayer = 0;
                        }
                        spun = wheel.Spin();
                    }

                    /* Draw the Puzzle Board and Standings */
                    ReDraw(false);

                    /* Get user input */
                    int correct = int.MinValue; // number of letters found by the player
                    while (correct == int.MinValue) // invalid letter
                    {
                        while (correct < 0)
                        {
                            ClearCurrentLine();
                            Console.Write(SzinesJatekos(currentPlayer) + ". játékos, adj meg egy mássalhangzót (" + String.Format(CI, "{0:C0}", spun) + "): ");
                            Console.ResetColor();
                            correct = board.Guess(Console.ReadKey().KeyChar, false);
                            if (correct == -2) // player hit 1
                            {
                                ClearCurrentLine();
                                Console.Write(SzinesJatekos(currentPlayer) + ". játékos, add meg a megoldást " + String.Format(CI, "{0:C0}", players[currentPlayer].Winnings) + "-ért: ");
                                Console.ResetColor();
                                if (!board.Guess(Console.ReadLine()))
                                {
                                    correct = 0;
                                    players[currentPlayer].Reset(); // if the player gets it wrong, they lose all their points
                                } else
                                {
                                    correct = 0;
                                    winner = currentPlayer; // board.gameOver is true, so we quit the main while loop
                                }
                                ReDraw(false);
                            } else if (correct == -3) // player hit 2
                            {
                                if (players[currentPlayer].Subtract(vowelPrice))
                                {
                                    ClearCurrentLine();
                                    Console.Write(SzinesJatekos(currentPlayer) + ". játékos, adj meg egy betűt: ");
                                    Console.ResetColor();
                                    board.Guess(Console.ReadKey().KeyChar, true);
                                    if (board.GameOver) // if someone buys the last missing vowel instead of solving the puzzle, they still win the game
                                    {
                                        correct = 0;
                                        winner = currentPlayer;
                                    }
                                }
                                ReDraw(false);
                            }
                        }
                    }
                    if (correct > 0) // at least one letter found
                    {
                        players[currentPlayer].Add(spun * correct);
                    }
                    else if (correct == 0) // no letters found -> next player
                    {
                        if (++currentPlayer == players.Length)
                        {
                            currentPlayer = 0;
                        }
                    }
                } // while (!board.GameOver)
                /* Only the winner of the round gets to keep their points */
                for (int j = 0; j < players.Length; j++)
                {
                    if (j != winner)
                    {
                        players[j].Reset();
                    }
                }
                /* Shop */
                ReDraw(true);
                int index = int.MinValue;
                while (index != -1 && players[winner].Winnings >= shop.LowestPrice())
                {
                    Console.Write("Választott termék: ");
                    index = int.Parse(Console.ReadLine()) - 1;
                    players[winner].Buy(shop, index);
                    ReDraw(true);
                }
            } // for (int i = 1; i <= rounds; i++)

            /* Final Results */
            Console.Clear();
            int finalWinner = 0;
            for (int j = 0; j < players.Length; j++)
            {
                Console.WriteLine(SzinesJatekos(j) + ". játékos: " + String.Format(CI, "{0:C0}", players[j].Winnings) + " " + String.Join(", ", players[j].Prizes));
                Console.ResetColor();
                if (players[j].Winnings > players[finalWinner].Winnings)
                {
                    finalWinner = j;
                }
            }
            Console.WriteLine();
            Console.WriteLine("Gratulálok, " + (finalWinner + 1) + ". játékos, nyertél!");
            Console.ReadKey();
        }
        static void ClearCurrentLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop);
        }
        static string SzinesJatekos(int i)
        {
            if ((i + 1) % 3 == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if ((i + 1) % 3 == 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            return (i+1).ToString();
        }
    }
}
