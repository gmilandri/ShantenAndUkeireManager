using System;
using System.Collections.Generic;

namespace ShantenAndUkeireManager
{
    public struct Tile : IComparable<Tile>
    {
        public readonly Suit Suit { get; }
        public readonly byte Rank { get; }
        public readonly bool IsRed { get; }

        public Tile(Suit suit, byte rank, bool isRed = false)
        {
            this.Suit = suit;
            this.Rank = rank;
            this.IsRed = isRed;
        }

        public Tile(char rank, Suit suit)
        {
            this.Suit = suit;
            if (rank - '0' == 0)
            {
                this.Rank = (byte)(rank - '0' + 5);
                this.IsRed = true;
            }
            else
            {
                this.Rank = (byte)(rank - '0');
                this.IsRed = false;
            }
        }

        public Tile (Tile tile)
        {
            this.Suit = tile.Suit;
            this.Rank = tile.Rank;
            this.IsRed = tile.IsRed;
        }

        public Tile (byte index)
        {
            this.IsRed = false;
            if (index < 10)
            {
                this.Suit = Suit.M;
                this.Rank = index;
            }
            else if (index < 30)
            {
                this.Suit = Suit.P;
                this.Rank = (byte)(index - 20);
            }
            else if (index < 50)
            {
                this.Suit = Suit.S;
                this.Rank = (byte)(index - 40);
            }
            else
            {
                this.Suit = Suit.Z;
                this.Rank = (byte)(index - 60);
            }
        }
  
        //public Tile(Mahjong.Model.Tile tile)
        //{
        //    this.Suit = (Suit)tile.Suit;
        //    this.Rank = tile.Rank;
        //    this.IsRed = tile.IsRed;
        //}

        public override bool Equals(Object obj)
        {
            return obj is Tile && Equals((Tile)obj);
        }

        public bool Equals(Tile other)
        {
            return Rank == other.Rank && Suit == other.Suit;
        }

        public int CompareTo(object obj)
        {
            if (obj is Tile)
                return CompareTo((Tile)obj);
            else
                throw new ArgumentException("Both objects being compared must be of type Tile");
        }

        public int CompareTo(Tile other)
        {
            int index1 = GetHashCode();
            int index2 = other.GetHashCode();
            if (index1 == index2)
                return 0;
            else if (index1 > index2)
                return 1;
            else
                return -1;
        }

        public static bool operator ==(Tile t1, Tile t2) => t1.Equals(t2);

        public static bool operator !=(Tile t1, Tile t2) => !t1.Equals(t2);

        public static bool operator <(Tile t1, Tile t2) => t1.CompareTo(t2) < 0;

        public static bool operator >(Tile t1, Tile t2) => t1.CompareTo(t2) > 0;

        public override int GetHashCode()
        {
            int index = Rank;
            if (Suit == Suit.P)
                index += 20;
            if (Suit == Suit.S)
                index += 40;
            if (Suit == Suit.Z)
                index += 60;
            return index;
        }

        public bool IsTerminal => !IsHonor && (Rank == 1 || Rank == 9);

        public bool IsHonor => Suit == Suit.Z;

        public bool IsWind => Suit == Suit.Z && Rank >= 1 && Rank <= 4;

        public bool IsDragon => Suit == Suit.Z && Rank >= 5 && Rank <= 7;

        public bool IsKokushiRelated => IsTerminal || IsHonor;

        public Tile Next()
        {
            Tile next;
            if (!IsHonor)
            {
                if (Rank != 9)
                    next = new Tile(Suit, (byte)(Rank + 1), IsRed);
                else
                    next = new Tile(Suit, 1);
            }
            else
            {
                if (IsWind && Rank != 4)
                    next = new Tile(Suit, (byte)(Rank + 1));
                else
                    next = new Tile(Suit, 1);
                if (IsDragon && Rank != 7)
                    next = new Tile(Suit, (byte)(Rank + 1));
                else
                    next = new Tile(Suit, 5);
            }
            return next;
        }

        public Tile Previous()
        {
            Tile next;
            if (!IsHonor)
            {
                if (Rank != 1)
                    next = new Tile(Suit, (byte)(Rank - 1), IsRed);
                else
                    next = new Tile(Suit, 9);
            }
            else
            {
                if (IsWind && Rank != 1)
                    next = new Tile(Suit, (byte)(Rank - 1));
                else
                    next = new Tile(Suit, 4);
                if (IsDragon && Rank != 5)
                    next = new Tile(Suit, (byte)(Rank - 1));
                else
                    next = new Tile(Suit, 7);
            }
            return next;
        }

        public static Tile RandomTile()
        {
            Random random = new Random();
            Suit mySuit = (Suit)random.Next(0, 4);
            int myRank = 0;
            if (mySuit != Suit.Z)
                myRank = random.Next(1, 10);
            else
                myRank = random.Next(1, 8);

            return new Tile(mySuit, (byte)myRank);
        }

        public static List<Tile> RandomHand()
        {
            var tempHand = new List<Tile>();
            for (int i = 0; i < 14; i++)
                tempHand.Add(RandomTile());
            tempHand.Sort();
            return tempHand;
        }

        public static HandValidity IsHandValid(List<Tile> tiles)
        {
            switch (tiles.Count)
            {
                case 2:
                case 5:
                case 8:
                case 11:
                case 14:
                    return HandValidity.Valid;
                case 1:
                case 4:
                case 7:
                case 10:
                case 13:
                    return HandValidity.MissingTile;
                default:
                    return HandValidity.Error;
            }
        }

        public static bool TryParse (string tile)
        {
            if (string.IsNullOrWhiteSpace(tile))
                return false;
            if (tile.Length != 2)
                return false;
            if (!char.IsDigit(tile[0]))
                return false;
            if (!char.IsLetter(tile[1]))
                return false;
            if (tile[1] == 'm' || tile[1] == 'p' || tile[1] == 's')
            {
                var number = Convert.ToInt32(tile[0] - '0');
                if (number > 0 && number < 10)
                    return true;
            }
            else if (tile[1] == 'z')
            {
                var number = Convert.ToInt32(tile[0] - '0');
                if (number > 0 && number < 8)
                    return true;
            }
            return false;
        }

        public readonly static Tile[] KokushiTiles = new Tile[]
{
            new Tile(Suit.M, 1),
            new Tile(Suit.M, 9),
            new Tile(Suit.P, 1),
            new Tile(Suit.P, 9),
            new Tile(Suit.S, 1),
            new Tile(Suit.S, 9),
            new Tile(Suit.Z, 1),
            new Tile(Suit.Z, 2),
            new Tile(Suit.Z, 3),
            new Tile(Suit.Z, 4),
            new Tile(Suit.Z, 5),
            new Tile(Suit.Z, 6),
            new Tile(Suit.Z, 7),

};

    }
    public enum Suit
    {
        M = 0,
        P = 1,
        S = 2,
        Z = 3
    }

    public enum HandValidity
    {
        Error = 0,
        MissingTile = 1,
        Valid = 2
    }

}
