using System;
using System.Collections.Generic;
using System.Text;

namespace ShantenAndUkeireManager
{
    public class HandCalculator
    {
        private int _shanten { get; set; }

        private List<Tile> _ukeire { get; set; }

        private List<HandInfo> _bestHands { get; set; }

        private List<Tile> _handTiles;

        public HandCalculator(List<Tile> tiles)
        {
            _handTiles = new List<Tile>(tiles);

        }

        public void MakeCalculations()
        {
            var bestHandsCalculator = new BestHandsCalculator(_handTiles);

            _bestHands = bestHandsCalculator.GetBestHands();

            _shanten = _bestHands[0].shanten;

            _ukeire = UkeireCalculator.DetermineUkeire(_bestHands);
        }

        public int GetShanten => _shanten;

        public List<Tile> GetUkeire => _ukeire;

    }
}
