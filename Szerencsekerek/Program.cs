using System;

namespace Szerencsekerek
{
    enum GuessType
    {
        nothing, consonant, vowel, solution, shop
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
                void ReDraw(GuessType guessType) // TODO: Find a better place for this function
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
                    if (guessType == GuessType.shop)
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
                    // ReDraw(false);

                    /* Get user input */
                    int correct = int.MinValue; // number of letters found by the player
                    while (correct == int.MinValue) // invalid letter
                    {
                        while (correct < 0)
                        {
                            ReDraw(GuessType.consonant);
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
                                ReDraw(GuessType.consonant);
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
                                ReDraw(GuessType.consonant);
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
                ReDraw(GuessType.shop);
                int index = int.MinValue;
                while (index != -1 && players[winner].Winnings >= shop.LowestPrice())
                {
                    Console.Write("Választott termék: ");
                    index = int.Parse(Console.ReadLine()) - 1;
                    players[winner].Buy(shop, index);
                    ReDraw(GuessType.shop);
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
