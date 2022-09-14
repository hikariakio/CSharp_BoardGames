public class TicTacToe : NRowGame
{
    #region COMMAND MESSAGES
    // COMMAND MESSAGES REGION
    readonly string helpMsg = "\n------------ HELP SECTION ------------\n" +
                           "\nThis is a help section of TicTacToe Game." +
                           "\nHelp section can be requested any time by typing \'help\'.\n" +
                           "\nAvailabe Commands\n" +
                           "\nrule - show rules of the current game.\n" +
                           "\nplay <x> <y> - play your current move. Making a move in TicTacToe will end your turn.\n" +
                           "\nsave <filename> - save your current game." +
                           "\nload <saved_filename> - load your saved game." +
                           "\nload -seeall - list all available saved files." +
                           "\n*while saving and loading,file extension is not required.*\n" +
                           "\nundo - undo your last move. Will not work on the first turn." +
                           "\nredo - redo your last undo-move. Will not work unless Undo is done first.\n" +
                           "\nshow - show the current game board and stats again." +
                           "\nquit - quit the game.";



    readonly string ruleMsg = "\n------------ RULE SECTION ------------\n" +
                              "\nTic-tac-toe is played on a three-by-three grid by two players alternately." +
                              "\nThe player who succeeds in placing THREE of their marks in a horizontal, vertical, or diagonal row is the winner";

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
                                            bool canTurnEnd = board!.IsInputCoordinateValid(coor);
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

    public override void InitBoard()
    {
        board = new TicTacToeBoard();
    }




    //Override Intelligent AI for TicTacToe Game
    protected override Coordinate AIMove()
    {
        return DoAlpBetPruning();
    }

    Coordinate DoAlpBetPruning()
    {

        float bestScore = -999999;
        Coordinate resultMove = new Coordinate();
        List<Coordinate> moves = board!.CalculateAvailableMoves(playerList[CurrentTurn].Piece);
        int turn = totalTurn;

        //seedMaxMove
        foreach (var move in moves)
        {
            float score = 0;

            totalTurn = turn;
            RegisterPiece(move);
            score = AlpBetPruning(99, -999999, 999999, false, CurrentTurn, move);
            //Console.WriteLine(score + " " + move); //debug
            RemovePiece(move);

            if (score > bestScore)
            {
                bestScore = score;
                resultMove = move;
            }
        }
        totalTurn = turn;

        return resultMove;
    }

    int AlpBetPruning(int depth, int alpha, int beta, bool maximize, int AInum, Coordinate chosenMove)
    {
        if (depth == 0)
        {
            return 0;
        }
        else
        if (board!.IsThereWinner(chosenMove, out int winTeam))
        {
            if (winTeam == AInum)
            {
                return 10 * depth;
            }
            else
            {
                return -10 * depth;
            }
        }
        else if (board.IsDraw())
        {
            return 0;
        }

        IncreaseTurn();

        if (maximize)
        {
            int bestScore = -999999;

            List<Coordinate> moves = board.CalculateAvailableMoves(playerList[CurrentTurn].Piece);
            int turn = totalTurn;
            foreach (var move in moves)
            {
                totalTurn = turn;
                RegisterPiece(move);
                int score = AlpBetPruning(depth - 1, alpha, beta, false, AInum, move);
                RemovePiece(move);

                if (score > bestScore)
                {
                    bestScore = score;
                }

                if (score > alpha)
                {
                    alpha = score;
                }

                if (beta <= alpha)
                    break;
            }
            return bestScore;
        }
        else
        {
            int bestScore = 999999;

            List<Coordinate> moves = board.CalculateAvailableMoves(playerList[CurrentTurn].Piece);
            int turn = totalTurn;
            foreach (var move in moves)
            {
                totalTurn = turn;
                RegisterPiece(move);
                int score = AlpBetPruning(depth - 1, alpha, beta, true, AInum, move); ;
                RemovePiece(move);
                if (score < bestScore)
                {
                    bestScore = score;
                }

                if (score < beta)
                {
                    beta = score;
                }

                if (beta <= alpha)
                    break;
            }
            return bestScore;

        }
    }
}
