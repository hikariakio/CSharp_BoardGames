class ReversiBoard : Board
{
   
    public ReversiBoard(int row=8, int col=8) : base(row, col)
    {
    }
    public ReversiBoard(Tile[] tiles,int row = 8, int col = 8) : base(tiles, row, col)
    {
    }

    public void SetupInitialPosition(Player[] playerList)
    {

        if (playerList.Length != 2 && (playerList[0] == null || playerList[1] == null))
        {
            Console.WriteLine("Not Setup players properly. Quitting the game!");
            return;
        }

        int firstPos = (maxRow - 1) / 2;
        int secondPos = firstPos + 1;

        GetTile(firstPos, secondPos).UpdatePiece(playerList[0].Piece);
        GetTile(secondPos, firstPos).UpdatePiece(playerList[0].Piece);

        GetTile(firstPos, firstPos).UpdatePiece(playerList[1].Piece);
        GetTile(secondPos, secondPos).UpdatePiece(playerList[1].Piece);
    }


    public override Board DeepClone()
    {
        Board clone = new ReversiBoard();

        if (Tiles.Length == clone.Tiles.Length)
        {
            for (int i = 0; i < Tiles.Length; i++)
            {
                clone.Tiles[i].UpdatePiece(Tiles[i].Piece);
            }
        }

        return clone;
    }

    public override void DisplayBoard()
    {
        Console.WriteLine();

        Console.Write(" Y→   ");
        for (int y = 0; y < maxCol; y++)
            Console.Write("{0}   ", y);

        Console.Write("\nX↓   ");
        for (int y = 0; y < maxCol; y++)
        {
            Console.Write("───");
            if (y != maxRow - 1) Console.Write("─");
        }
        Console.WriteLine();

        for (int x = 0; x < maxRow; x++)
        {
            Console.Write(" {0}  │ ", x);
            for (int y = 0; y < maxCol; y++)
            {
                if (!GetTile(x, y).IsEmpty())
                {
                    Console.Write(GetTile(x, y).Piece.Shape);
                }
                else
                {
                    Console.Write("·");
                }
                Console.Write(" │ ");
            }
            Console.Write("\n     ");
            for (int y = 0; y < maxCol; y++)
            {
                Console.Write("───");
                if (y != maxRow - 1) Console.Write("─");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

  


    public override bool IsInputCoordinateValid(Coordinate coordinate,Piece? currentPiece)
    {
        if (currentPiece == null)
            return false;
        return GetFlippedMoves(coordinate, currentPiece).Count > 0;        
    }


    //In reversi, board cannot tell if there is available moves or not
    //neither which player is winning, it only holds information of pieces.
    //Draw will be known after game over., so default is false
    public override bool IsDraw()
    {
        return false;
    }

    public override bool IsThereWinner(Coordinate inputMove)
    {
        return IsThereWinner(inputMove,out _);
    }

    public override bool IsThereWinner(Coordinate inputMove, out int winTeam)
    {
        winTeam = -1;
        return false;
    }

    public List<Coordinate> GetFlippedMoves(Coordinate startCoordinate, Piece currentPiece)
    {
        List<Coordinate> flippedMoves = new List<Coordinate>();
        if( !IsOutOfBound(startCoordinate) && GetTile(startCoordinate).IsEmpty() )
        {
            Coordinate[] directions =
                {
                    new Coordinate(0,1),
                    new Coordinate(1,1),
                    new Coordinate(1,0),
                    new Coordinate(1,-1),
                    new Coordinate(0,-1),
                    new Coordinate(-1,-1),
                    new Coordinate(-1,0),
                    new Coordinate(-1,1),
                };

            GetTile(startCoordinate).UpdatePiece(currentPiece);

            

            foreach (Coordinate dir in directions)
            {
           
                Coordinate track = startCoordinate;
                track += dir;



                if (!IsOutOfBound(track) && !GetTile(track).IsEmpty() && !GetTile(track).Piece.Equals(currentPiece)) //== otherPiece
                {
                    track += dir;
                    if (IsOutOfBound(track))
                        continue;

                    while (!GetTile(track).IsEmpty() && !GetTile(track).Piece.Equals(currentPiece) ) // == otherPiece
                    {
                        track += dir;
                        if (IsOutOfBound(track))
                            break;
                    }
                    if (IsOutOfBound(track))
                        continue;

                    if( !GetTile(track).IsEmpty() && GetTile(track).Piece.Equals(currentPiece))
                    {
                        while(true)
                        {
                            track -= dir;
                            if (track == startCoordinate)
                            {
                                break;
                            }                                
                            flippedMoves.Add(track);
                        }
                    }
                }
            }
            GetTile(startCoordinate).RemovePiece();

        }

        return flippedMoves;
    }
}
