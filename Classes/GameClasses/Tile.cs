public class Tile
{
    Coordinate coordinate;

    Piece piece;

    public Piece Piece { get => piece; }
    public Coordinate Coordinate { get => coordinate; }


    public Tile(Coordinate _coordinate)
    {
        coordinate = _coordinate;
    }

    public Tile(int x, int y)
    {

        coordinate = new Coordinate(x, y);
    }


    public void UpdatePiece(Piece _piece)
    {
        piece = _piece;
    }

    public void RemovePiece()
    {
        piece = null;
    }

    public bool IsEmpty()
    {
      return piece == null; 
    }
}
