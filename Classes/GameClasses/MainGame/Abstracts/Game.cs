public abstract class Game
{
   
    protected int totalTurn = 0;

    protected Coordinate lastMove; // trackLastMove

    protected Board? board ;
    protected Player[]? playerList ;

    protected bool isAnyWinner = false;
    protected bool isGameOver;

    protected List<Board> moveTrackList = new List<Board>();
    protected int trackIndex = 0; // counter for moveTrackList
    
    public bool IsGameOver { get => isGameOver; }
    public int TotalTurn { get => totalTurn; }
    public Player[]? PlayerList { get => playerList; }
    public Board? Board { get => board; }
    protected int CurrentTurn { get => (totalTurn % playerList.Length); }
    protected CommandList gameCommandList = new CommandList();


    //every game has diff boards
    public abstract void InitBoard();
    //every game has diff commands
    public abstract void GenerateCommmandList();
    //every game shows different infos
    public abstract void ShowGameInfo();
    //every games evaluate differently
    protected abstract void UpdateGameState();
    //every game has diff rule of getting winner.
    protected abstract Player GetWinnerPlayer();

    public virtual void LoadGame(int totalTurn, bool isGameOver, Player[] playerList, Board board)
    {
        this.totalTurn = totalTurn;
        this.isGameOver = isGameOver;
        this.playerList = playerList;
        this.board = board;

        moveTrackList.Clear();
        trackIndex = 0;
    }

    public void InitPlayers(int playerCount = 2)
    {
        this.playerList = Player.CreatePlayers(playerCount);
    }

  
    //Override this to implement new variants of AI
    protected virtual Coordinate AIMove()
    {
        return RandomizeAnAvailableMove();
    }

    protected virtual void MakeValidMove(Coordinate coordinate)
    {
        //Before Move, Tracker will save the current state.
        TrackState();
        //During Move , Put the piece at coor
        RegisterPiece(coordinate);
        //After Move , Evalulate the game.
        UpdateGameState();

        if (!isGameOver)
            IncreaseTurn();
    }


    protected void TrackState()
    {
        //input new move will forget everything from the current state.
        moveTrackList.RemoveRange(trackIndex, moveTrackList.Count - trackIndex);
        //and add new move to Tracker
        moveTrackList.Add(board!.DeepClone());
        trackIndex++;
    }


    //UNDO And REDO will do step-2 when they are performed
    //i.e skipped back to your prev turn.
    protected void UndoState()
    {
        //no more move to undo
        if (trackIndex - 2 < 0)
        {
            Console.WriteLine("Cannot Undo. Previous Move not found!");
        }
        else
        {
            
            //add current state to list iff it is the last move
            if (trackIndex == moveTrackList.Count)
            {
                moveTrackList.Add(board!.DeepClone());
            }
            totalTurn -= 2;
            trackIndex -= 2;
            board = moveTrackList[trackIndex];
        }

    }

    protected void RedoState()
    {
        //no more move to redo
        if (trackIndex + 2 > moveTrackList.Count)
        {
            Console.WriteLine("Cannot Redo. Already at Last Move");
        }
        else
        {
            totalTurn += 2;
            trackIndex += 2;
            board = moveTrackList[trackIndex];
        }

    }



    public void ExecutePlayerTurn() 
    {
        switch (playerList[CurrentTurn].PlayerMode)
        {
            case Player.Mode.AI:
                MakeValidMove(AIMove());
                break;

            case Player.Mode.human:
            default:
                InputManager.PromptCommand(gameCommandList);
                break;
        }
    }

    

    public void IncreaseTurn() // nout turn go
    {
        totalTurn++;
    }


    public void RegisterPiece(Coordinate _coordinate)
    {
        lastMove = _coordinate;
        board.GetTile(_coordinate.X, _coordinate.Y).UpdatePiece(playerList[CurrentTurn].Piece);

    }

    public void RemovePiece(Coordinate _coordinate)
    {
        board.GetTile(_coordinate.X, _coordinate.Y).RemovePiece();
    }

    //Normal AI Move
    protected Coordinate RandomizeAnAvailableMove()
    {
        List<Coordinate> moves = board.CalculateAvailableMoves(playerList[CurrentTurn].Piece);
        int i = new Random().Next(moves.Count);
        return moves[i];
    }

    

    protected void ShowGameTurnText()
    {
        if (!isGameOver)
            Console.WriteLine($" >>>   {playerList![CurrentTurn].PlayerName}'s Turn ({playerList[CurrentTurn].Piece.Shape})   <<<");

    }

    public virtual void ShowGameOverText()
    {
        if (isAnyWinner)
            Console.WriteLine($" > > > {GetWinnerPlayer().PlayerName} WINS!! < < <");
        else if (isGameOver)
            Console.WriteLine(" > > > IT IS A DRAW!! < < < ");
    }

}
