namespace AdventOfCode.Dec3
{
    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Location))
            {
                return false;
            }

            return Equals((Location) obj);
        }

        protected bool Equals(Location other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X*397) ^ Y;
            }
        }

        public Location Clone()
        {
            return new Location {X = X, Y = Y};
        }
    }
}