using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace ShantenAndUkeireManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This program should find the shanten of a hand and the ukeire tiles.");
            Console.WriteLine("The input should be in the following format: 22m13468p3s11225z");
            Console.WriteLine("Awaiting Input...");

            var tiles = MyConverter.StringToTileConverter(Console.ReadLine());

            var discardOptimizerManager = new DiscardOptimizerManager(tiles);
            discardOptimizerManager.MakeCalculations();

            Console.WriteLine("\nThe shanten is:" + discardOptimizerManager.Shanten);

            List<Tile> discardedTiles = new List<Tile>();

            foreach (var discardInfo in discardOptimizerManager.DiscardsInfo)
            {
                if (discardedTiles.Contains(discardInfo.DiscardedTile))
                    continue;
                discardedTiles.Add(discardInfo.DiscardedTile);
                Console.WriteLine("\nBy discarding " + discardInfo.DiscardedTile.Rank.ToString() + discardInfo.DiscardedTile.Suit.ToString().ToLower());
                Console.WriteLine("The ukeire tiles (" + discardInfo.TotalUkeireTiles.ToString() + ") are: ");
                Console.WriteLine(MyConverter.ConvertTilesToTenhouFormat(discardInfo.Ukeire));
            }
        }
    }
}
