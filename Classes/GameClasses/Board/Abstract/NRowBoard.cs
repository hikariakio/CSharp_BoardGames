public abstract class NRowBoard : Board
{
    //make N-in-a-row to win
    //Derive from this class for games like Gomoku , connect 49  etc.
    //tic-tac-toe = 3
    //connect4 = 4
    int n;


    protected NRowBoard(int row, int col,int n) : base(row, col)
    {
        this.n = n;
    }


    protected NRowBoard(Tile[] tiles,int row,int col, int n) : base(tiles,row,col)
    {
        this.n = n;
    }


    // in NRow games, game is a draw when there are no available moves anymore and no winner.
    public override bool IsDraw()
    {
        foreach (Tile tile in tiles)
        {
            if (tile.IsEmpty())
            {
                return false;
            }
        }
        return true;
    }

    public override bool IsThereWinner(Coordinate inputMove)
    {
        return IsThereWinner(inputMove, out _);
    }

    public override bool IsThereWinner(Coordinate inputMove, out int winningTeam)
    {
        winningTeam = -1;

        List<Tile[]> moves = GetPossibleWinCheckSets(inputMove);

        for (int i = 0; i < moves.Count; i++)
        {
            Tile[] oneSet = moves[i];

            if (AreWinCheckSetsEmpty(oneSet))
            {
                continue;
            }

            if (DoesWinCheckSetsHaveSamePieces(oneSet))
            {
                winningTeam = oneSet[0].Piece.Team;
                return true;
            }
        }

        return false;
    }

    
    // FUTURE : Tweak here for generate your custom winning list.
    // CURRENT: Has four directions.
    protected List<Tile[]> GetPossibleWinCheckSets(Coordinate inputMove)
    {
        List<Tile[]> winningMoves = new List<Tile[]>();
       
        int x = inputMove.X;
        int y = inputMove.Y;

        //// | direction

        for (int i = -(n - 1); i <= 0; i++)
        {
            int startX = x + i;

            if (startX >= 0) // Asa ka Baung htal mhr so
            {
                int endX = startX + n;

                if (endX <= maxRow) // asone ka baung htl mhr so
                {
                    Tile[] setMoves = new Tile[n];
                    for (int j = 0; j < n; j++)
                    {
                        setMoves[j] = GetTile(startX + j, y);
                    }
                    winningMoves.Add(setMoves);
                }

            }

        }

        //// - direction

        for (int i = -(n - 1); i <= 0; i++)
        {
            int startY = x + i;
            if (startY >= 0)
            {
                int endY = startY + n;
                if (endY <= maxCol)
                {
                    Tile[] setMoves = new Tile[n];
                    for (int j = 0; j < n; j++)
                    {
                        setMoves[j] = GetTile(x, startY + j);
                    }
                    winningMoves.Add(setMoves);
                }

            }

        }

        //// \ direction

        for (int i = -(n - 1); i <= 0; i++)
        {
            int startX = x + i;
            int startY = y + i;
            if (startX >= 0 && startY >= 0)
            {
                int endX = startX + n;
                int endY = startY + n;

                if (endX <= maxRow && endY <= maxCol) //board col + 1
                {
                    Tile[] setMoves = new Tile[n];
                    for (int j = 0; j < n; j++)
                    {
                        setMoves[j] = GetTile(startX + j, startY + j);
                    }
                    winningMoves.Add(setMoves);
                }
            }
        }


        //// / direction

        for (int i = -(n - 1); i <= 0; i++)
        {
            int startX = x + i;
            int startY = y - i;

            if (startY < maxCol && startX >= 0)
            {

                int endX = startX + n;
                int endY = startY - n;


                if (endX <= maxRow && endY >= -1) //board col + 1
                {
                    Tile[] setMoves = new Tile[n];
                    for (int j = 0; j < n; j++)
                    {
                        setMoves[j] = GetTile(startX + j, startY - j);
                    }
                    winningMoves.Add(setMoves);
                }
            }
        }



        return winningMoves;
    }

    protected bool AreWinCheckSetsEmpty(Tile[] set)
    {
        for (int i = 0; i < set.Length; i++)
        {
            if (set[i].IsEmpty())
            {
                return true;
            }
        }
        return false;
    }

    protected bool DoesWinCheckSetsHaveSamePieces(Tile[] set)
    {
        for (int i = 0; i < set.Length - 1; i++)
        {
            if (set[i].Piece.Team != set[i + 1].Piece.Team)
                return false;
        }
        return true;
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

}

