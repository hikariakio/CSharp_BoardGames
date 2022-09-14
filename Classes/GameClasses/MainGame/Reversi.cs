public class Reversi : Game
{

    #region COMMAND MESSAGES
    // COMMAND MESSAGES REGION
    readonly string helpMsg = "\n------------ HELP SECTION ------------\n" +
                           "\nThis is a help section of Reversi Game." +
                           "\nHelp section can be requested any time by typing \'help\'.\n" +
                           "\nAvailabe Commands\n" +
                           "\nrule - show rules of the current game.\n" +
                           "\nplay <x> <y> - play your current move. Making a move in Reversi will end your turn.\n" +
                           "\nsave <filename> - save your current game." +
                           "\nload <saved_filename> - load your saved game." +
                           "\nload -seeall - list all available saved files." +
                           "\n*while saving and loading,file extension is not required.*\n" +
                           "\nundo - undo your last move. Will not work on the first turn." +
                           "\nredo - redo your last undo-move. Will not work unless Undo is done first.\n" +
    "\nshow - show the current game board and stats again." +
    "\nquit - quit the game.";




    readonly string ruleMsg = "\n------------ RULE SECTION ------------\n" +
                             "\nEach piece played must be laid adjacent to an opponent's piece so that the opponent's piece or a row of opponent's pieces is flanked by the new piece and another piece of the player's piece.All of the opponent's pieces between these two pieces are 'captured' and turned over to match the player's piece." +
                              "\nIt can happen that a piece is played so that pieces or rows of pieces in more than one direction are trapped between the new piece played and other pieces of the same piece.In this case, all the pieces in all viable directions are turned over." +
                               "\nThe game is over when neither player has a legal move (i.e.a move that captures at least one opposing piece) or when the board is full.";

    readonly string quitMsg = "\n\tThank you for playing <3.";

    readonly string playUsage = "\tusage :play <x> <y> ...";
    readonly string playWarning = "\twarning :Invalid Move. Type 'rule' for more info ...";

    readonly string saveUsage = "\tusage :save <savefilename> , extension not required ...";
    readonly string loadUsage = "\tusage :load <savefilename> , extension not required ...";

    #endregion


    public override void GenerateCommmandList()
    {
        gameCommandList = new CommandList(
                                        new NormalPrintCommand(helpMsg, "help"),
                                        new NormalPrintCommand(ruleMsg, "rule"),

                                        new ArgsCommand(args =>
                                        {
                                            Coordinate coor = Coordinate.GetCoordinateFromArgs(args);
                                            //Invalid means Not ending turn.
                                            bool canTurnEnd = board!.IsInputCoordinateValid(coor, playerList[CurrentTurn].Piece);
                                            if (canTurnEnd)
                                            {
                                                MakeValidMove(coor);
                                            }
                                            else
                                            {
                                                Console.WriteLine(playWarning);
                                            }
                                            return canTurnEnd;
                                        },
                                        "play",
                                        playUsage
                                        ),

                                        new NoArgsCommand(() =>
                                        {
                                            ShowGameInfo();
                                            return false;
                                        },
                                        "show"),

                                        new ArgsCommand(args =>
                                        {
                                            StateManager.SaveGameState(args);
                                            return false;
                                        },
                                        "save",
                                        saveUsage
                                        ),

                                        new ArgsCommand(args =>
                                        {
                                            return StateManager.LoadGameState(args);
                                        },
                                        "load",
                                        loadUsage
                                        ),

                                        new NoArgsCommand(() =>
                                        {
                                            Console.WriteLine(quitMsg);
                                            Environment.Exit(0);
                                            return false;
                                        },
                                        "quit"),

                                        new NoArgsCommand(() =>
                                        {
                                            UndoState();
                                            return true;
                                        },
                                        "undo"),

                                        new NoArgsCommand(() =>
                                        {
                                            RedoState();
                                            return true;
                                        },
                                        "redo")
                                       );

        gameCommandList.ExtractCommand("help").Execute();

    }


    int[] scoreList;

    public override void InitBoard()
    {
        ReversiBoard board = new ReversiBoard();
        board.SetupInitialPosition(playerList);
        this.board = board;
        scoreList = new int[playerList.Length];
        UpdateScore();
    }



    public override void LoadGame(int totalTurn, bool isGameOver, Player[] playerList, Board board)
    {
        base.LoadGame(totalTurn, isGameOver, playerList, board);
        scoreList = new int[playerList.Length];
    }

    protected override void UpdateGameState()
    {
        FlipPieces();
        UpdateScore();

        isGameOver = ((ReversiBoard)board).CalculateAvailableMoves(playerList![(CurrentTurn + 1) % playerList.Length].Piece).Count == 0;

        isAnyWinner = (IsGameOver && scoreList[0] != scoreList[1]);
    }


    public override void ShowGameInfo()
    {
        board.DisplayBoard();
        ShowGameTurnText();
        UpdateScore(); //for redo and undo
        Console.WriteLine($"{playerList[0].PlayerName} : {scoreList[0]}  | |  {scoreList[1]} : {playerList[1].PlayerName}");
    }



    // at the game end, player with more score is winner
    protected override Player GetWinnerPlayer()
    {
        return scoreList[0] > scoreList[1] ? playerList![0] : playerList![1];
    }


    void FlipPieces()
    {

        //tempSet Blank
        board.GetTile(lastMove).RemovePiece();

        foreach (var flippedMove in ((ReversiBoard)board).GetFlippedMoves(lastMove, playerList![CurrentTurn].Piece))
        {
            board.GetTile(flippedMove).UpdatePiece(playerList![(CurrentTurn) % playerList.Length].Piece);
        }

        //tempSet Restore
        board.GetTile(lastMove).UpdatePiece(playerList![CurrentTurn].Piece);

    }


    void UpdateScore()
    {
        if (playerList.Length == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < playerList.Length; i++)
            {
                int score = 0;
                foreach (var tile in board.Tiles)
                {
                    if (tile.Piece != null)
                    {
                        if (tile.Piece.Team == playerList[i].Piece.Team)
                        {
                            score++;
                        }
                    }
                }
                scoreList[i] = score;
            }
        }


    }

}