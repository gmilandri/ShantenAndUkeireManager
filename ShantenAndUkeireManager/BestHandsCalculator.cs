﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace ShantenAndUkeireManager
{
    public class BestHandsCalculator
    {
        private HandInfo _startingHand;
        private readonly List<HandInfo> _bestHands;
        private int _bestShanten;

        public  BestHandsCalculator(List<Tile> tiles)
        {
            _startingHand = new HandInfo(tiles);
            _bestHands = new List<HandInfo>();
            _bestShanten = 99;
            DetermineBestHands();           
        }

        public List<HandInfo> GetBestHands()
        {
            return _bestHands;
        }

        private void DetermineBestHands ()
        {
            Queue shantenCounterQueue = new Queue();

            HandInfo allPairsHand;
            int shantenPairs = _startingHand.DetermineAllPairsShanten(out allPairsHand);
            HandInfo kokushiHand;
            int shantenKokushi = _startingHand.DetermineKokushiShanten(out kokushiHand);
            if (shantenPairs <= _bestShanten)
            {
                _bestShanten = shantenPairs;
                _bestHands.Add(allPairsHand);
            }
            if (shantenKokushi < _bestShanten)
            {
                _bestShanten = shantenKokushi;
                _bestHands.Clear();
                _bestHands.Add(kokushiHand);
            }
            if (shantenKokushi == _bestShanten)
            {
                _bestHands.Add(kokushiHand);
            }

            shantenCounterQueue.Enqueue(_startingHand);

            do
            {
                var currentHand = (HandInfo)shantenCounterQueue.Dequeue();

                if (_bestHands.Count > 0 && currentHand.isolatedTiles.Count > _bestHands[_bestHands.Count - 1].isolatedTiles.Count)
                {
                    continue;
                }
                int currentHandShanten;
                var newPossibleHands = new List<HandInfo>();
                if (currentHand.remainingTiles.Count != 0)
                {
                    currentHand.ElaborateHand(out newPossibleHands, out currentHandShanten);
                }
                else
                {
                    currentHand.DetermineShanten();
                    currentHandShanten = currentHand.shanten;
                }

                if (currentHandShanten < _bestShanten && currentHand.remainingTiles.Count == 0)
                {
                    _bestShanten = currentHandShanten;
                    _bestHands.Clear();
                    _bestHands.Add(currentHand);
                }
                else if (currentHandShanten == _bestShanten && currentHand.remainingTiles.Count == 0)
                {
                    _bestHands.Add(currentHand);
                }
                if (newPossibleHands != null)
                {
                    foreach (var hand in newPossibleHands)
                        shantenCounterQueue.Enqueue(hand);
                }
            }
            while (shantenCounterQueue.Count != 0);

        }
    }
}