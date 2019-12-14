using System;
using System.Collections.Generic;
using System.Linq;

namespace ShantenAndUkeireManager
{
    static class UkeireCalculator
    {
        public static List<Tile> DetermineUkeire (List<HandInfo> hands)
        {
            var allHandsUkeire = new List<Tile>();

            foreach (var hand in hands)
            {
                var ukeire = hand.ukeire;
                if (!hand.kokushi && !hand.allPairs)
                {
                    foreach (var unfinishedMeld in hand.unfinishedMelds)
                    {
                        if (unfinishedMeld[0] == unfinishedMeld[1])
                            ukeire.Add(unfinishedMeld[0]);
                        else if (unfinishedMeld[0] == unfinishedMeld[1].Previous())
                        {
                            if (unfinishedMeld[0].Rank != 8)
                                ukeire.Add(unfinishedMeld[0].Next().Next());
                            if (unfinishedMeld[0].Rank != 1)
                                ukeire.Add(unfinishedMeld[0].Previous());
                        }
                        else if (unfinishedMeld[0] == unfinishedMeld[1].Previous().Previous())
                            ukeire.Add(unfinishedMeld[0].Next());
                    }
                    if (hand.numberOfMelds + hand.unfinishedMelds.Count < 4)
                    {
                        foreach (var isolatedTile in hand.isolatedTiles)
                        {
                            ukeire.Add(isolatedTile);

                            if (!isolatedTile.IsHonor)
                            {
                                switch (isolatedTile.Rank)
                                {
                                    case 1:
                                        ukeire.Add(isolatedTile.Next());
                                        ukeire.Add(isolatedTile.Next().Next());
                                        break;
                                    case 2:
                                        ukeire.Add(isolatedTile.Previous());
                                        ukeire.Add(isolatedTile.Next());
                                        ukeire.Add(isolatedTile.Next().Next());
                                        break;
                                    case 8:
                                        ukeire.Add(isolatedTile.Previous().Previous());
                                        ukeire.Add(isolatedTile.Previous());
                                        ukeire.Add(isolatedTile.Next());
                                        break;
                                    case 9:
                                        ukeire.Add(isolatedTile.Previous().Previous());
                                        ukeire.Add(isolatedTile.Previous());
                                        break;
                                    default:
                                        ukeire.Add(isolatedTile.Previous().Previous());
                                        ukeire.Add(isolatedTile.Previous());
                                        ukeire.Add(isolatedTile.Next());
                                        ukeire.Add(isolatedTile.Next().Next());
                                        break;
                                }
                            }
                        }
                    }
                    else if (!hand.hasPair)
                    {
                        foreach (var tile in hand.isolatedTiles)
                            ukeire.Add(tile);
                    }
                }
                else if (hand.kokushi)
                {
                    if (!hand.hasPair)
                    {
                        foreach (var tile in Tile.KokushiTiles)
                            ukeire.Add(tile);
                    }
                    else
                    {
                        foreach (var tile in Tile.KokushiTiles)
                            ukeire.Add(tile);
                        foreach (var tile in hand.remainingTiles)
                        {
                            if (ukeire.Contains(tile))
                                ukeire.Remove(tile);
                        }
                    }
                }
                else if (hand.allPairs)
                {
                    foreach (var tile in hand.isolatedTiles)
                        ukeire.Add(tile);
                }
            }

            foreach (var handInfo in hands)
            {
                foreach (var tile in handInfo.ukeire)
                {
                    if (!allHandsUkeire.Contains(tile))
                    {
                        allHandsUkeire.Add(tile);
                    }
                }
            }

            allHandsUkeire.Sort();

            return allHandsUkeire;
        }
    }
}
