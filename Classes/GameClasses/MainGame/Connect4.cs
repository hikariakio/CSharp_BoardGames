public class Connect4 : NRowGame
{
    #region COMMAND MESSAGES
    // COMMAND MESSAGES REGION
    readonly string helpMsg = "\n------------ HELP SECTION ------------" +
                           "\nThis is a help section of Connect4 Game." +
                           "\nHelp section can be requested any time by typing \'help\'.\n" +
                           "\nAvailabe Commands\n" +
                           "\nrule - show rules of the current game.\n" +
                           "\nplay <X> <Y> - play your current move. Making a move in Connect4 will end your turn.\n" +
                           "\nsave <filename> - save your current game." +
                           "\nload -seeall - list all available saved files." +
                           "\n*while saving and loading,file extension is not required.*\n" +
                           "\nundo - undo your last move. Will not work on the first turn." +
                           "\nredo - redo your last undo-move. Will not work unless Undo is done first.\n" +
                           "\nshow - show the current game board and stats again.";


    readonly string ruleMsg = "\n------------ RULE SECTION ------------\n" +
                             "\ntConnect4 is played on a seven-by-six grid by two players alternately." +
                             "\nThe player who succeeds in placing FOUR of their marks in a horizontal, vertical, or diagonal row is the winner." +
                             "\nThe pieces fall straight down, occupying the lowest available space within the column.";
  

    readonly string playUsage = "\tusage :play <x> <y> ...";
    readonly string playWarning = "\twarning :Invalid Move. Type 'rule' for more info ...";

    readonly string quitMsg = "\n\tThank you for playing <3.";

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

                                         new NoArgsCommand(() => { ShowGameInfo(); return false; }, "show"),

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
        board = new Connect4Board();
    }
}
