public struct Coordinate : IEquatable<Coordinate>
{
    int x = -99;
    int y = -99;

    public int X { get => x; }
    public int Y { get => y; }

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string? ToString()
    {
        return ($"X:{x}|Y:{y} ");
    }



    #region operands

    public static Coordinate operator +(Coordinate a) => a;
    public static Coordinate operator -(Coordinate a) => new Coordinate(-a.x, -a.y);

    public static Coordinate operator +(Coordinate a, Coordinate b)
    => new Coordinate(a.x + b.x, a.y + b.y);

    public static Coordinate operator -(Coordinate a, Coordinate b)
    => a + (-b);

    public bool Equals(Coordinate other)
    {
        return x == other.x && y == other.y;
    }

    public static bool operator ==(Coordinate lhs, Coordinate rhs)
    {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Coordinate lhs, Coordinate rhs) => !(lhs == rhs);
    #endregion


    public static Coordinate GetCoordinateFromArgs(string[] args)
    {
        if (args != null && args.Length == 2)
        {
            int[] intArray = new int[2];

            if (InputManager.AreInputsNumbers(args, out intArray))
            {
                Coordinate coordinate = new Coordinate(intArray[0], intArray[1]);
                return coordinate;
            }
        }
        return new Coordinate(-99,-99);
    }

 
}