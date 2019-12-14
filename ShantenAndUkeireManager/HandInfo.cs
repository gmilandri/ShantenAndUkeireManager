using System;
using System.Collections.Generic;
using System.Linq;

namespace ShantenAndUkeireManager
{
    public class HandInfo
    {
        public List<Tile> remainingTiles;
        public int numberOfMelds = 0;
        public List<Tile[]> unfinishedMelds;
        public List<Tile> isolatedTiles;
        public bool hasPair;

        public int previousShanten;
        public int maxShanten = 8;

        public List<Tile> ukeire = new List<Tile>();
        public int shanten;

        public bool kokushi;
        public bool allPairs;


        public HandInfo(List<Tile> tiles)
        {
            remainingTiles = new List<Tile>(tiles);
            unfinishedMelds = new List<Tile[]>();
            isolatedTiles = new List<Tile>();
            numberOfMelds = (13 - tiles.Count) / 3; ;
        }

        public HandInfo(List<Tile> precedentTiles, int precedentNumberOfMelds, List<Tile[]> precedentUnfinishedMelds, List<Tile> precedentIsolatedTiles, bool precedentHasPair, int precedentShanten, int maxShanten)
        {
            remainingTiles = new List<Tile>(precedentTiles);
            numberOfMelds = precedentNumberOfMelds;
            unfinishedMelds = new List<Tile[]>(precedentUnfinishedMelds);
            isolatedTiles = new List<Tile>(precedentIsolatedTiles);
            hasPair = precedentHasPair;
            previousShanten = precedentShanten;
            this.maxShanten = maxShanten;
            DetermineShanten();
        }

        public void DetermineShanten ()
        {
            shanten = DetermineStandardShanten();
        }

        public int DetermineKokushiShanten(out HandInfo kokushiHand)
        {
            kokushiHand = new HandInfo(remainingTiles);
            if (kokushiHand.remainingTiles.Count >= 13)
            {
                int answer = 13;

                for (int i = kokushiHand.remainingTiles.Count -1; i >= 0; i--)
                {
                    if (!kokushiHand.remainingTiles[i].IsKokushiRelated)
                    {
                        kokushiHand.isolatedTiles.Add(kokushiHand.remainingTiles[i]);
                        kokushiHand.remainingTiles.RemoveAt(i);
                    }
                }

                if (kokushiHand.IsThereAnyPair())
                {
                    kokushiHand.hasPair = true;
                    answer--;
                }

                kokushiHand.remainingTiles = RemoveAllDoubles(kokushiHand.remainingTiles);

                answer -= kokushiHand.remainingTiles.Count;

                kokushiHand.shanten = answer;

                return answer;
            }
            else
                return maxShanten;
        }

        public bool IsThereAnyPair()
        {
            var answer = false;
            for (var i = 0; i < remainingTiles.Count - 1; i++)
            {
                if (remainingTiles[i] == remainingTiles[i + 1])
                {
                    answer = true;
                    break;
                }
            }
            return answer;
        }

        public static List<Tile> RemoveAllDoubles (List<Tile> oldTiles) => oldTiles.Distinct().ToList();

        public int DetermineStandardShanten()
        {
            int answer = maxShanten;
            answer -= numberOfMelds * 2 + unfinishedMelds.Count;
            answer += hasPair ? -1 : 0;

            return answer;
        }

        public int DetermineAllPairsShanten(out HandInfo allPairsHand)
        {
            allPairsHand = new HandInfo(remainingTiles);
            if (remainingTiles.Count >= 13)
            {
                int numberOfPairs = 0;
                List<Tile> pairTiles = new List<Tile>();

                for (int tile = 0; tile < remainingTiles.Count - 1; tile++)
                {
                    if (remainingTiles[tile] == remainingTiles[tile +1] && !pairTiles.Contains(remainingTiles[tile]))
                    {
                        tile++;
                        numberOfPairs++;
                        pairTiles.Add(remainingTiles[tile]);
                    }
                    else if (!pairTiles.Contains(remainingTiles[tile]))
                        allPairsHand.isolatedTiles.Add(remainingTiles[tile]);
                }

                allPairsHand.allPairs = true;
                allPairsHand.shanten = 6 - numberOfPairs;
                return 6 - numberOfPairs;
            }
            else
                return maxShanten;
        }

        public void ElaborateHand (out List<HandInfo> newPossibleHands, out int tempShanten)
        {
            Tile currentTile = remainingTiles[0];
            newPossibleHands = new List<HandInfo>();
            HandInfo newSetHand;
            if (remainingTiles.Count >= 3 && numberOfMelds + unfinishedMelds.Count <= 3)
            {
                CheckSet(currentTile, out newSetHand);
                if (newSetHand != null)
                {
                    newPossibleHands.Add(newSetHand);
                }
                if (currentTile.Suit != Suit.Z)
                {
                    CheckChi(currentTile, out newSetHand);
                    if (newSetHand != null)
                    {
                        newPossibleHands.Add(newSetHand);
                    }
                }
            }
            if (remainingTiles.Count >= 2)
            {
                if (!hasPair)
                {
                    CheckPair(currentTile, out newSetHand);
                    if (newSetHand != null)
                    {
                        newPossibleHands.Add(newSetHand);
                    }
                }
                if (numberOfMelds + unfinishedMelds.Count <= 3)
                {
                    CheckUnfinishedPon(currentTile, out newSetHand);
                    if (newSetHand != null)
                    {
                        newPossibleHands.Add(newSetHand);
                    }
                    if (currentTile.Suit != Suit.Z)
                    {
                        CheckUnfinishedChiOne(currentTile, out newSetHand);
                        if (newSetHand != null)
                        {
                            newPossibleHands.Add(newSetHand);
                        }
                        CheckUnfinishedChiTwo(currentTile, out newSetHand);
                        if (newSetHand != null)
                        {
                            newPossibleHands.Add(newSetHand);
                        }
                    }
                }
            }

            CheckIsolatedTile(currentTile, out newSetHand);
            if (newSetHand != null)
                newPossibleHands.Add(newSetHand);

            DetermineShanten();
            tempShanten = shanten;
            return;
        }

        public void CheckSet (Tile tile, out HandInfo newPossibleHand)
        {
            if (remainingTiles[1] == tile && remainingTiles[2] == tile)
            {
                List<Tile> newTiles = new List<Tile>();
                for (int i = 3; i < remainingTiles.Count; i++)
                    newTiles.Add(remainingTiles[i]);
                newPossibleHand = new HandInfo(newTiles, numberOfMelds + 1, unfinishedMelds, isolatedTiles, hasPair, shanten, maxShanten);
            }
            else
                newPossibleHand = null;
        }

        public void CheckChi (Tile tile, out HandInfo newPossibleHand)
        {
            if (remainingTiles.Contains(tile.Next()) && remainingTiles.Contains(tile.Next().Next()))
            {
                List<Tile> newTiles = new List<Tile>(remainingTiles);
                newTiles.Remove(tile);
                newTiles.Remove(tile.Next());
                newTiles.Remove(tile.Next().Next());
                newPossibleHand = new HandInfo(newTiles, numberOfMelds + 1, unfinishedMelds, isolatedTiles, hasPair, shanten, maxShanten);
            }
            else
                newPossibleHand = null;
        }

        public void CheckPair (Tile tile, out HandInfo newPossibleHand)
        {
            if (remainingTiles.IndexOf(tile) != remainingTiles.LastIndexOf(tile))
            {
                int indexOne = remainingTiles.IndexOf(tile);
                int indexTwo = remainingTiles.LastIndexOf(tile);
                List<Tile> newTiles = new List<Tile>(remainingTiles);
                newTiles.RemoveAt(indexTwo);
                newTiles.RemoveAt(indexOne);
                newPossibleHand = new HandInfo(newTiles, numberOfMelds, unfinishedMelds, isolatedTiles, true, shanten, maxShanten);
            }
            else
                newPossibleHand = null;
        }

        public void CheckUnfinishedPon (Tile tile, out HandInfo newPossibleHand)
        {
            if (remainingTiles.IndexOf(tile) != remainingTiles.LastIndexOf(tile))
            {
                int indexOne = remainingTiles.IndexOf(tile);
                int indexTwo = remainingTiles.LastIndexOf(tile);
                List<Tile> newTiles = new List<Tile>(remainingTiles);
                List<Tile[]> newUnfinishedMelds = new List<Tile[]>(unfinishedMelds);
                newUnfinishedMelds.Add(new Tile[] { remainingTiles[indexOne], remainingTiles[indexTwo] });
                newTiles.RemoveAt(indexTwo);
                newTiles.RemoveAt(indexOne);
                newPossibleHand = new HandInfo(newTiles, numberOfMelds, newUnfinishedMelds, isolatedTiles, hasPair, shanten, maxShanten);
            }
            else
                newPossibleHand = null;
        }

        public void CheckUnfinishedChiOne(Tile tile, out HandInfo newPossibleHand)
        {
            if (remainingTiles.Contains(tile.Next()))
            {
                List<Tile> newTiles = new List<Tile>(remainingTiles);
                List<Tile[]> newUnfinishedMelds = new List<Tile[]>(unfinishedMelds);
                newUnfinishedMelds.Add(new Tile[] { tile, tile.Next() });
                newTiles.Remove(tile);
                newTiles.Remove(tile.Next());
                newPossibleHand = new HandInfo(newTiles, numberOfMelds, newUnfinishedMelds, isolatedTiles, hasPair, shanten, maxShanten);
            }
            else
                newPossibleHand = null;
        }

        public void CheckUnfinishedChiTwo(Tile tile, out HandInfo newPossibleHand)
        {
            if (remainingTiles.Contains(tile.Next().Next()))
            {
                List<Tile> newTiles = new List<Tile>(remainingTiles);
                List<Tile[]> newUnfinishedMelds = new List<Tile[]>(unfinishedMelds)
                {
                    new Tile[] { tile, tile.Next().Next() }
                };
                newTiles.Remove(tile);
                newTiles.Remove(tile.Next().Next());
                newPossibleHand = new HandInfo(newTiles, numberOfMelds, newUnfinishedMelds, isolatedTiles, hasPair, shanten, maxShanten);
            }
            else
                newPossibleHand = null;
        }

        public void CheckIsolatedTile(Tile tile, out HandInfo newPossibleHand)
        {
            List<Tile> newTiles = new List<Tile>(remainingTiles);
            newTiles.Remove(tile);
            List<Tile> newIsolatedTiles = new List<Tile>(isolatedTiles);
            newIsolatedTiles.Add(tile);
            newPossibleHand = new HandInfo(newTiles, numberOfMelds, unfinishedMelds, newIsolatedTiles, hasPair, shanten, maxShanten);
        }


    }
}
