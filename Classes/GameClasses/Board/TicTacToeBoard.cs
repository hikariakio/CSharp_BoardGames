class TicTacToeBoard : NRowBoard
{

    public TicTacToeBoard(int row = 3, int col = 3, int n = 3) : base(row, col, n)
    {

    }

    public TicTacToeBoard(Tile[] tiles, int row = 3, int col = 3, int n = 3) : base(tiles, row, col, n)
    {

    }

    public override bool IsInputCoordinateValid(Coordinate coordinate, Piece? currentPiece = null)
    {
        return !IsOutOfBound(coordinate) && GetTile(coordinate).IsEmpty();
    }

    public override Board DeepClone()
    {
        Board clone = new TicTacToeBoard();

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
