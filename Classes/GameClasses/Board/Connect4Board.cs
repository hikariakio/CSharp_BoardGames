class Connect4Board : NRowBoard
{
    
    public Connect4Board(int row = 6, int col = 7, int n=4) : base(row, col, n)
    {
    }

    public Connect4Board(Tile[] tiles,int row=6,int col = 7, int n = 4) : base(tiles, row,col,n)
    {
    }

    public override bool IsInputCoordinateValid(Coordinate coordinate,Piece? currentPiece = null)
    {
        return !IsOutOfBound(coordinate) && GetTile(coordinate).IsEmpty() && IsBottomTile(coordinate);
    }

    bool IsBottomTile(Coordinate coordinate)
    {
        int x = coordinate.X;
        int y = coordinate.Y;

        return x == maxRow - 1 || !GetTile(x + 1, y).IsEmpty();        
    }

  


    public override Board DeepClone()
    {
        Board clone = new Connect4Board();

        if (Tiles.Length == clone.Tiles.Length)
        {
            for (int i = 0; i < Tiles.Length; i++)
            {
                clone.Tiles[i].UpdatePiece(Tiles[i].Piece);
            }
        }

        return clone;
    }
}
