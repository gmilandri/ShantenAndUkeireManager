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
            string inputFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Mahjongtests\input.txt";
            string outputFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Mahjongtests\output.txt";

            StringBuilder log = new StringBuilder();
            Console.WriteLine("This program should find the shanten of a hand and the ukeire tiles.");
            Console.WriteLine("The content of the file should be in the following format: 22m13468p3s11225z");
            Console.WriteLine("I'm looking into the file: " + inputFilePath);

            var tiles = MyConverter.FileContentConverter(inputFilePath);

            log.Append(MyConverter.ConvertTilesToTenhouFormat(tiles));

            var discardOptimizerManager = new DiscardOptimizerManager(tiles);
            discardOptimizerManager.MakeCalculations();

            log.Append("\n\nThe shanten is:" + discardOptimizerManager.Shanten);
            log.Append("\n");

            List<Tile> discardedTiles = new List<Tile>();

            foreach (var discardInfo in discardOptimizerManager.DiscardsInfo)
            {
                if (discardedTiles.Contains(discardInfo.DiscardedTile))
                    continue;
                discardedTiles.Add(discardInfo.DiscardedTile);
                log.Append("\nBy discarding " + discardInfo.DiscardedTile.Rank.ToString() + discardInfo.DiscardedTile.Suit.ToString().ToLower());
                log.Append("\nThe ukeire tiles (" + discardInfo.TotalUkeireTiles.ToString() + ") are: ");
                log.Append(MyConverter.ConvertTilesToTenhouFormat(discardInfo.Ukeire));
                log.Append("\n");
            }
            File.WriteAllText(outputFilePath, log.ToString(), Encoding.UTF8);
        }
    }
}
