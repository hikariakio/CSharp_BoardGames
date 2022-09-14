public class InputManager
{

    public static void PromptCommand(CommandList commands)
    {
        ICommand cmd;
        do
        {
            Console.Write("\n$ ");
            string? commandString = Console.ReadLine();
            cmd = Classify(commandString!.ToLower(), commands);
            cmd.Execute();
        }
        while (!cmd.IsEndTurnCommand); //inputMove is given //todo
    }

    static ICommand Classify(string commandString, CommandList commands)
    {

        (string mainCommand,string[] arguments) = SplitCmdAndArgs(commandString);
        return commands.ExtractCommand(mainCommand, arguments);
    }



    static (string, string[]) SplitCmdAndArgs(string commandString)
    {
        string[] split = commandString.Split(' ');
        
        string mainCommand = split[0];
        string[]? arguments = null;
        if (split.Length > 1)
        {
            arguments = commandString.Substring(split[0].Length + 1).Split(' ');
        }

        return (mainCommand!,arguments!);
    }

    public static string PromptNonEmptyString(string promptMsg= "")
    {
        string? message;
        do
        {
            Console.WriteLine("\n"+promptMsg);
            Console.Write(">> ");
            message =  Console.ReadLine();
        }
        while (string.IsNullOrWhiteSpace(message));

        return message;
    }

    public static T PromptEnum<T>(string promptMsg ="") where T : Enum
    {
        Array array = Enum.GetValues(typeof(T));

        string? mode;
        int modeInt;
        do
        {
            Console.WriteLine("\n" +promptMsg);
            for (int i = 0; i < array.Length; i++)
            {
                
                Console.Write("{0} - {1} | ", i, array.GetValue(i));
            }
            Console.WriteLine();
            Console.Write(">> ");
            mode = Console.ReadLine();
        }
        while (!int.TryParse(mode, out modeInt) || (modeInt < 0 || modeInt >= array.Length));
        return (T)(object)modeInt;
    }

    public static bool AreInputsNumbers(string[] inputArray, out int[] intArray)
    {
        intArray = new int[inputArray.Length];

        for (int i = 0; i < inputArray.Length; i++)
        {
            int output;
            if (int.TryParse(inputArray[i], out output))
            {
                intArray[i] = output;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}