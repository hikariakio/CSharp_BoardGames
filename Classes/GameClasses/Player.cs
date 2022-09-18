public class Player
{
    static char[] ShapesCollection = { 'O', 'X', 'U', 'H', 'T', 'V', 'M', '♥', '☻', '♦', '♠', 'Φ', 'Ω', 'φ' }; //add more to support more players

    public enum Mode { human, AI };

    string playerName;
    Mode playerMode;
    Piece piece;

    public string PlayerName { get => playerName; }
    public Mode PlayerMode { get => playerMode; }
    public Piece Piece { get => piece; }


    public Player(string playerName, int playerNumber, Mode mode, char shape)
    {
        this.playerName = playerName;
        this.playerMode = mode;
        this.piece = new Piece(playerNumber, shape);
    }

    static char[] GetRandomShapes(int num)
    {
        char[] shapes = new char[num];
        int randnum = new Random().Next(ShapesCollection.Length);
        for (int i = 0; i < num; i++)
        {
            shapes[i] = ShapesCollection[(randnum + i) % ShapesCollection.Length];
        }
        return shapes;
    }


    public static Player[] CreatePlayers(int playerCount)
    {
        Player[] playerList = new Player[playerCount];

        char[] shapes = GetRandomShapes(playerCount);
        for (int i = 0; i < playerCount; i++)
        {
            //Parsing Index for better UI in ordering
            string name = InputManager.PromptNonEmptyString($"Enter a player#{i + 1}'s name.");
            Player.Mode mode = InputManager.PromptEnum<Mode>($"Enter a player#{i + 1}'s mode.");

            playerList[i] = new Player(name, i, mode, shapes[i]);
        }

        return playerList;
    }

}