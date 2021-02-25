using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ShantenAndUkeireManager
{
    //THIS CLASS SHOULD TAKE A LIST OF TILES AND PROVIDE THE FOLLOWING:
    //SHANTEN
    //A LIST OF TILES TO DISCARD TO IMPROVE THE HAND
    //ORDERED BY THE UKEIRE TILES COUNT
    class DiscardOptimizerManager
    {
        private List<Tile> _completeHand;

        public int Shanten { get; private set; }

        public List<DiscardOptimizer> DiscardsInfo;

        public DiscardOptimizerManager(List<Tile> tiles)
        {
            _completeHand = new List<Tile>(tiles);
            DiscardsInfo = new List<DiscardOptimizer>();

        }
        public void MakeCalculations()
        {
            var handCalculator = new HandCalculator(_completeHand);
            handCalculator.MakeCalculations();
            Shanten = handCalculator.GetShanten;

            foreach (var tile in _completeHand)
            {
                DiscardOptimizer discardOptimizer = new DiscardOptimizer(_completeHand, tile, Shanten);
                discardOptimizer.DetermineFitness();
                if (discardOptimizer.IsFit)
                    DiscardsInfo.Add(discardOptimizer);
            }

            DiscardsInfo = DiscardsInfo.OrderByDescending(discardInfo => discardInfo.TotalUkeireTiles).ToList();

            for (int i = DiscardsInfo.Count - 1; i >= 0; i--)
            {
                if (DiscardsInfo[i].TotalUkeireTiles == 0)
                    DiscardsInfo.RemoveAt(i);
            }
        }
    }
}
