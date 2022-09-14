// This class behaves like mainmenu
// This Match will be Singleton Pattern : Can only have one game instance per programm
public sealed class Match
{

    #region COMMAND MESSAGES
    // COMMAND MESSAGES REGION
    readonly string mainMenuHelpMsg = "\nThis is a help section of Main Menu." +
                           "\nHelp section can be requested any time by typing \'help\'.\n" +
                           "\nAvailabe Commands\n" +
                           "\ninfo - show info and credit about this game." +
                           "\nplay - choose your game!." +
                           "\nquit - quit the game.";

    readonly string selectionHelpMsg = "\nThis is a help section of Choosing Game." +
                           "\nHelp section can be requested any time by typing \'help\'.\n" +
                           "\nAvailabe Commands\n" +
                           "\nnewgame - start a brand new game." +
                           "\nload <savedFileName> - continue playing your saved game!." +
                           "\nload -seeall - list all available saved files." +
                           "\nquit - quit the game.";

    readonly string infoMsg = "\nCredit\n" +
                              "\nThis project is created by Ye Gaung Kyaw(n10923543) for IFN563-OOD Assignment." +
                              "\nIt is developed using C# .Net 6.0" +
                              "\nCurrently,It features 3 board games.";

    readonly string loadUsage = "\tusage :load <savefilename> , extension not required ...";


    #endregion

    enum GameType
    {
        TicTacToe,
        Connect4,
        Reversi
    };



    // Singleton Pattern
    // The whole entire program can only have one game per time.
    private Match () { } 

    private static Match? instance;

    public static Match? GetInstance()
    {
        if (instance == null)
        {
            instance = new Match();
        }
        return instance;
    }

    Game game;
    public Game Game { get => game; }

    CommandList GenerateMainMenuCommmands()
    {
        CommandList mainmenuCommand = new CommandList
                                 (
        new NormalPrintCommand(mainMenuHelpMsg, "help"),
        new NoArgsCommand(() => { ChooseGame(); return true; }, "play"), // create a new game can end prompting.
        new NormalPrintCommand(infoMsg, "info"),
        new NoArgsCommand(() => { Environment.Exit(0); return false; }, "quit")
                                  );

        mainmenuCommand.ExtractCommand("help").Execute();
        return mainmenuCommand;
    }

    CommandList GenerateSelectionCommmands()
    {
        CommandList mainmenuCommand = new CommandList
                                 (
                                    new NormalPrintCommand(selectionHelpMsg, "help"),

                                    new NoArgsCommand(() =>
                                    {
                                        StartNewGame();
                                        return true;
                                    },
                                    "newgame"
                                    ), // create a new game can end prompting.

                                    new NormalPrintCommand(infoMsg, "info"),

                                    new ArgsCommand((args) =>
                                    {
                                        return StateManager.LoadGameState(args);
                                    },
                                    "load",
                                    loadUsage
                                    ),

                                    new NoArgsCommand(() =>
                                    {
                                        Environment.Exit(0);
                                        return false;
                                    },
                                    "quit")
                                  ); ;

        mainmenuCommand.ExtractCommand("help").Execute();

        return mainmenuCommand;
    }


    void ShowWelcomeMessage()
    {
        Console.WriteLine("****************************************");
        Console.WriteLine("*                                      *");
        Console.WriteLine("*  WELCOME to C# BoardGame Collection  *");
        Console.WriteLine("*      Developed by Ye Gaung Kyaw.     *");
        Console.WriteLine("*                                      *");
        Console.WriteLine("***************************************");

    }

    void EnterMainMenuSelction()
    {
        //will only break after seleting a game
        InputManager.PromptCommand(GenerateMainMenuCommmands());
        //will only break after creating or loading a game
        InputManager.PromptCommand(GenerateSelectionCommmands());
    }

    public void SpawnGame()
    {
        ShowWelcomeMessage();
        EnterMainMenuSelction();

        // failing to create a game will halt the program
        if (game == null)
            return;

        while (!Game.IsGameOver)
        {
            Game.ShowGameInfo();      // Show Board,ScoreLists and other info.
            Game.ExecutePlayerTurn(); // Wait Player Input or calculate AI moves
            if (game.IsGameOver)
            {
                Game.ShowGameInfo();
                Game.ShowGameOverText();
            }

        }
    }

    void ChooseGame()
    {
        GameType type = InputManager.PromptEnum<GameType>("Choose your game mode. Only enter a number.");

        if (type == GameType.TicTacToe)
            game = new TicTacToe();
        else if (type == GameType.Connect4)
            game = new Connect4();
        else if (type == GameType.Reversi)
            game = new Reversi();
    }

    void StartNewGame()
    {
        game.InitPlayers(2);
        game.InitBoard();
        game.GenerateCommmandList();
    }

}