using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShantenAndUkeireManager
{
    public static class MyConverter
    {
        public static List<Tile> FileContentConverter(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The file indicated does not exist in the desktop as of now. Please create a txt file named hand.", filePath);

            string tiles = File.ReadAllText(filePath).Trim();

            if (String.IsNullOrWhiteSpace(tiles))
                throw new Exception("The specified file is empty.");

            var answer = new List<Tile>();

            string[] hand;
            if (tiles.Contains("m"))
            {
                hand = tiles.Split('m');

                for (int i = 0; i < hand[0].Length; i++)
                    answer.Add(new Tile(hand[0][i], Suit.M));

                tiles = hand[1];
            }

            if (tiles.Contains("p"))
            {
                hand = tiles.Split('p');

                for (int i = 0; i < hand[0].Length; i++)
                    answer.Add(new Tile(hand[0][i], Suit.P)); ;

                tiles = hand[1];
            }

            if (tiles.Contains("s"))
            {
                hand = tiles.Split('s');

                for (int i = 0; i < hand[0].Length; i++)
                    answer.Add(new Tile(hand[0][i], Suit.S)); ;

                tiles = hand[1];
            }

            if (tiles.Contains("z"))
            {
                hand = tiles.Split('z');

                for (int i = 0; i < hand[0].Length; i++)
                    answer.Add(new Tile(hand[0][i], Suit.Z)); ;
            }

            switch (answer.Count)
            {
                case 2:
                case 5:
                case 8:
                case 11:
                case 14:
                    break;
                default:
                    throw new Exception("There must be 2, 5, 8, 11 or 14 tiles.");
            }

            answer.Sort();

            return answer;
        }

        public static StringBuilder ConvertTilesToTenhouFormat (List<Tile> tiles)
        {
            StringBuilder tenhouFormat = new StringBuilder();

            for (int i = 0; i < tiles.Count; i++)
            {
                if (i == tiles.Count-1)
                {
                    tenhouFormat.Append(tiles[i].Rank.ToString() + tiles[i].Suit.ToString().ToLower());
                }
                else
                {
                    tenhouFormat.Append(tiles[i].Rank.ToString());
                    if (tiles[i].Suit != tiles[i+1].Suit)
                    {
                        tenhouFormat.Append(tiles[i].Suit.ToString().ToLower());
                    }
                }
            }

            return tenhouFormat;
        }
    }
}
