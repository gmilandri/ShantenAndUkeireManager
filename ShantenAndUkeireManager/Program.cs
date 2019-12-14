using System;
using System.Collections.Generic;

namespace ShantenAndUkeireManager
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("This program should find the shanten of a hand and the ukeire tiles.");
                Console.WriteLine("The input should be in the following format: 22m13468p3s11225z");

                var tiles = new List<Tile>();

                while (true)
                {
                    if (tiles.Count == 0)
                    {
                        Console.WriteLine("Awaiting Input...");
                        tiles = MyConverter.StringToTileConverter(Console.ReadLine().Trim().ToLowerInvariant());
                    }

                    switch (Tile.IsHandValid(tiles))
                    {
                        case HandValidity.Valid:
                            ShowShantenAndUkeire(tiles);
                            break;
                        case HandValidity.MissingTile:
                            Console.WriteLine("Please enter the tile you want to draw. Any invalid entry will be treated as a random tile.");
                            string newDrawTile = Console.ReadLine();

                            if (Tile.TryParse(newDrawTile))
                                tiles.Add(MyConverter.StringToTileConverter(newDrawTile.Trim().ToLowerInvariant())[0]);
                            else
                                tiles.Add(Tile.RandomTile());
                            tiles.Sort();

                            ShowShantenAndUkeire(tiles);
                            break;
                        case HandValidity.Error:
                            Console.WriteLine("The tiles were not in a valid state.");
                            break;
                    }

                    Console.WriteLine("\nPress y to continue.");
                    if (Console.ReadLine().Trim().ToLowerInvariant() != "y")
                        break;

                    Console.WriteLine("Please enter the tile you want to discard. Any invalid entry will be treated as a random tile.");
                    string newDiscardTile = Console.ReadLine();

                    if (Tile.TryParse(newDiscardTile))
                    {
                        Tile discard = MyConverter.StringToTileConverter(newDiscardTile.Trim().ToLowerInvariant())[0];
                        if (tiles.Contains(discard))
                            tiles.Remove(discard);
                    }

                    switch (Tile.IsHandValid(tiles))
                    {
                        case HandValidity.Valid:
                            var random = new Random();
                            int randomTile = random.Next(0, tiles.Count);
                            tiles.RemoveAt(randomTile);
                            break;
                        case HandValidity.MissingTile:
                            break;
                        case HandValidity.Error:
                            Console.WriteLine("The tiles were not in a valid state.");
                            break;
                    }

                    Console.WriteLine("The tiles now are: " + MyConverter.ConvertTilesToTenhouFormat(tiles));
                }
            }
            catch (Exception)
            {
                Console.WriteLine("An unexpected error has occured.");
            }
            finally
            {
                Console.WriteLine("\n\nThanks for using this program <3");
            }
        }

        static private void ShowShantenAndUkeire (List<Tile> tiles)
        {
            var discardOptimizerManager = new DiscardOptimizerManager(tiles);
            discardOptimizerManager.MakeCalculations();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nThe shanten is:" + discardOptimizerManager.Shanten);
            Console.ResetColor();

            List<Tile> discardedTiles = new List<Tile>();

            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var discardInfo in discardOptimizerManager.DiscardsInfo)
            {
                if (discardedTiles.Contains(discardInfo.DiscardedTile))
                    continue;
                discardedTiles.Add(discardInfo.DiscardedTile);
                Console.WriteLine("\nBy discarding " + discardInfo.DiscardedTile.Rank.ToString() + discardInfo.DiscardedTile.Suit.ToString().ToLower());
                Console.WriteLine("The ukeire tiles (" + discardInfo.TotalUkeireTiles.ToString() + ") are: ");
                Console.WriteLine(MyConverter.ConvertTilesToTenhouFormat(discardInfo.Ukeire));
            }
            Console.ResetColor();
        }
    }
}
