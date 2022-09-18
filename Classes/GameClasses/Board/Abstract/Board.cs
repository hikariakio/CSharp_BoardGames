public abstract class Board
{
    protected Tile[] tiles;
    public Tile[] Tiles { get => tiles; }

    protected int maxRow;
    protected int maxCol;

    protected Board(int mRow , int mCol)
    {
        this.maxRow = mRow;
        this.maxCol = mCol;
        Tile[] tiles = new Tile[maxRow * maxCol];
        for (int i = 0; i < maxRow * maxCol; i++)
        {         
            Coordinate coordinate = new Coordinate(i/maxCol, i % maxCol);
            Tile tile = new Tile(coordinate);
            tiles[i] = tile;            
        }
        this.tiles = tiles;
    }

    protected Board(Tile[] tiles,int mRow,int mCol)
    {
        this.maxRow = mRow;
        this.maxCol = mCol;
        this.tiles = tiles;        
    }

    public abstract bool IsDraw();
    public abstract Board DeepClone();
    public abstract bool IsInputCoordinateValid(Coordinate coordinate,Piece? currentPieceInPlay = null);

    public abstract bool IsThereWinner(Coordinate inputMove);
    public abstract bool IsThereWinner(Coordinate inputMove, out int winTeam);
    public abstract void DisplayBoard();

    

    public Tile GetTile( Coordinate coordinate)
    {
        return tiles[coordinate.X * maxCol + coordinate.Y];
    }

    public Tile GetTile(int x ,int y)
    {
        return tiles[x *maxCol + y];
    }

    public bool IsOutOfBound(Coordinate coordinate)
    {
        return coordinate.X < 0 || coordinate.X >= maxRow || coordinate.Y < 0 || coordinate.Y >= maxCol;
    }

    public List<Coordinate> CalculateAvailableMoves(Piece? currentPieceInPlay)
    {
        List<Coordinate> moves = new List<Coordinate>();
        foreach (Tile tile in tiles)
        {
            if (IsInputCoordinateValid(tile.Coordinate, currentPieceInPlay))
            {
                moves.Add(tile.Coordinate);
            }
        }
        return moves;
    }
}

