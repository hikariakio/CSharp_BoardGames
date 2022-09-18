public class Piece
{
    int team;
    char shape;

    public int Team { get => team;}
    public char Shape { get => shape;  }

    public Piece(int _team,char _shape)
    {
        this.team = _team;
        this.shape = _shape;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        return team == ((Piece)obj).team;

    }


}