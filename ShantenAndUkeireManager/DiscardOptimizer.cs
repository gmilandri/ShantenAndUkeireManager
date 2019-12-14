using System;
using System.Collections.Generic;
using System.Text;

namespace ShantenAndUkeireManager
{
    //THIS CLASS SHOULD DETERMINE IF A HAND IS AT LEAST EQUAL OR BETTER THAN THE ORIGINAL
    //IF IT IS BETTER, DETERMINE THE UKEIRE OF SAID HAND AND THE COUNT OF SUCH TILES
    class DiscardOptimizer
    {
        private List<Tile> _completeHand;

        private int _originalShanten;

        private int _newShanten;

        public int TotalUkeireTiles { get; private set; }

        public List<Tile> Ukeire { get; private set; }

        public Tile DiscardedTile { get; private set; }

        public bool IsFit { get; private set; }

        public DiscardOptimizer(List<Tile> tiles, Tile discardedTile, int originalShanten)
        {
            _completeHand = new List<Tile>(tiles);
            DiscardedTile = discardedTile;
            _originalShanten = originalShanten;
        }

        public void DetermineFitness()
        {
            _completeHand.Remove(DiscardedTile);
            HandCalculator handCalculator = new HandCalculator(_completeHand);
            handCalculator.MakeCalculations();
            Ukeire = handCalculator.GetUkeire;
            _newShanten = handCalculator.GetShanten;
            if (_newShanten == _originalShanten)
            {
                IsFit = true;
                DetermineTotalUkeireTiles();
            }
        }
        public void DetermineTotalUkeireTiles()
        {
            TotalUkeireTiles = Ukeire.Count * 4;
            foreach (var ukeireTile in Ukeire)
            {
                foreach (var tile in _completeHand)
                {
                    if (tile == ukeireTile)
                        TotalUkeireTiles--;
                }
            }
        }

    }
}
