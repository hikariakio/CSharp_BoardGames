
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

//this class entangles with Match(Singleton) Class
//Save and Load StateManager 
public class StateManager
{

    public static void SaveGameState(string[] args)
    {
        //nullrefCheck
        if (args == null)
            return;

        string saveFileName = args[0];

        //blank check
        if (string.IsNullOrWhiteSpace(saveFileName))
            return;


        //do not want to accept special characters
        if (saveFileName.Any(ch => !Char.IsLetterOrDigit(ch)))
        {
            Console.WriteLine("\tSpecial Characters Detected! Cancel Saving File.");
            return;
        }

        string mainPath = Directory.GetCurrentDirectory();
        string saveFile = Path.Combine(mainPath, saveFileName + ".yg" + Match.GetInstance().Game.GetType().ToString());


        if (File.Exists(saveFile))
        {
            string overWriteChar = InputManager.PromptNonEmptyString("File Already Exists! 'Y' to overwrite or 'N' to cancel.");
            if (overWriteChar.ToLower() != "y")
            {
                Console.WriteLine("\tCancel Saving File. Returning to game!");
                return;
            }
        }

        string json = JsonConvert.SerializeObject(Match.GetInstance().Game);
        StreamWriter saveWriter = new StreamWriter(saveFile);
        saveWriter.Write(Match.GetInstance().Game.GetType());
        saveWriter.Write("\n");
        saveWriter.Write(json);
        saveWriter.Close();
        Console.WriteLine("\tGame Saved Successfully. You may exit the game.");

    }

    public static bool LoadGameState(string[] args)
    {
        //nullrefCheck
        if (args == null)
            return false;

        string saveFileName = args[0];

        //blank check
        if (string.IsNullOrWhiteSpace(saveFileName))
            return false;

        string mainPath = Directory.GetCurrentDirectory();

        if (saveFileName == "-seeall")
        {

            string[] files = Directory.GetFiles(mainPath, "*.yg" + Match.GetInstance().Game.GetType().ToString());
            if (files == null || files.Length == 0)
            {
                Console.WriteLine("\tThere is no saved file for this game type!");
            }
            else
            {
                Console.WriteLine("\nFound {0} saved files!\n", files.Length);
                for (int i = 0; i < files.Length; i++)
                {
                    Console.WriteLine("\t{0}", Path.GetFileNameWithoutExtension(files[i]));
                }
            }
            return false;
        }


        string savedFile = Path.Combine(mainPath, saveFileName + ".yg" + Match.GetInstance().Game.GetType().ToString());
        if (!File.Exists(savedFile))
        {
            Console.WriteLine("\nCannot locate the saved file named \"{0}\"", Path.GetFileNameWithoutExtension(savedFile));
            Console.WriteLine("\"load -seeall\" to list all available saved files.");
            return false;
        }

        StreamReader loadReader = new StreamReader(savedFile);

        string[] jsoninfo = loadReader.ReadToEnd().Split('\n');
        string gameType = jsoninfo[0];


        string gameInfo = jsoninfo[1];

        JObject gameInfoJobj = JObject.Parse(gameInfo);
        bool isGameOver = gameInfoJobj["IsGameOver"]!.ToObject<bool>();
        int totalTurn = gameInfoJobj["TotalTurn"]!.ToObject<int>();
        int playerCount = gameInfoJobj["PlayerList"]!.Count();
        Player[] playerList = new Player[playerCount];
        for (int i = 0; i < playerCount; i++)
        {
            string name = gameInfoJobj["PlayerList"]![i]!["PlayerName"]!.ToString();
            int teamNum = gameInfoJobj["PlayerList"]![i]!["Piece"]!["Team"]!.ToObject<int>();
            int mode = gameInfoJobj["PlayerList"]![i]!["PlayerMode"]!.ToObject<int>();
            char shape = gameInfoJobj["PlayerList"]![i]!["Piece"]!["Shape"]!.ToObject<char>();

            playerList[i] = new Player(name, teamNum, (Player.Mode)mode, shape);
        }
        int tileCount = gameInfoJobj["Board"]!["Tiles"]!.Count();
        Tile[] tiles = new Tile[tileCount];

        for (int i = 0; i < tileCount; i++)
        {
            int x = gameInfoJobj["Board"]!["Tiles"]![i]!["Coordinate"]!["X"]!.ToObject<int>();
            int y = gameInfoJobj["Board"]!["Tiles"]![i]!["Coordinate"]!["Y"]!.ToObject<int>();

            Coordinate coordinate = new Coordinate(x, y);
            tiles[i] = new Tile(coordinate);

            if (gameInfoJobj["Board"]!["Tiles"]![i]!["Piece"]!.Type != JTokenType.Null)
            {
                int teamNum = gameInfoJobj["Board"]!["Tiles"]![i]!["Piece"]!["Team"]!.ToObject<int>();
                char shape = gameInfoJobj["Board"]!["Tiles"]![i]!["Piece"]!["Shape"]!.ToObject<char>();
                Piece piece = new Piece(teamNum, shape);
                tiles[i].UpdatePiece(piece);
            }
        }

        Board? board = null;

        if (gameType == "TicTacToe")
        {
            board = new TicTacToeBoard(tiles);
        }
        else if (gameType == "Connect4")
        {
            board = new Connect4Board(tiles);
        }
        else if (gameType == "Reversi")
        {
            board = new ReversiBoard(tiles);

        }
        else
        {
            return false;
        }
        Match.GetInstance().Game.LoadGame(totalTurn, isGameOver, playerList, board);
        Match.GetInstance().Game.GenerateCommmandList();
        Console.WriteLine("\nGame Loaded Successfully!");
        loadReader.Close();
        return true;
    }
}