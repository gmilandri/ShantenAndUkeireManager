﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShantenAndUkeireManager
{
    public static class MyConverter
    {
        public static List<Tile> StringToTileConverter(string tiles)
        {
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
