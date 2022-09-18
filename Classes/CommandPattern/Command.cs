
public interface ICommand
{
    // EndingTurn useful in games that can skip turns ( reversi or uno )
    // or after getting valid move
    // or loading a game succesffly

    bool IsEndTurnCommand { get; }

    void Execute();
    bool IsApplicable(string cmd, string[]? args);
}



//Print Command will never end Turn;
public class NormalPrintCommand : ICommand
{
    public bool IsEndTurnCommand => false;

    string printMsg;
    string cmdKeyWord;

    public NormalPrintCommand(string printMsg, string cmdKeyWord)
    {
        this.printMsg = printMsg;
        this.cmdKeyWord = cmdKeyWord;
    }

    public void Execute()
    {
        Console.WriteLine(printMsg);
    }
    public bool IsApplicable(string cmd, string[]? args)
    {
        if (cmd.ToLower() == cmdKeyWord)
            return true;
        return false;
    }
}

//args no important in those commands
public class NoArgsCommand : ICommand
{
    bool isEndTurnCommand;
    public bool IsEndTurnCommand => isEndTurnCommand;
    string cmdKeyWord;

    //output True will end the turn.
    Func<bool> execMethod;


    public NoArgsCommand(Func<bool> execMethod, string cmdKeyWord)
    {
        this.execMethod = execMethod;
        this.cmdKeyWord = cmdKeyWord;
    }

    public void Execute()
    {
        isEndTurnCommand = execMethod();
    }
    public bool IsApplicable(string cmd, string[]? args)
    {
        if (cmd.ToLower() == cmdKeyWord)
            return true;
        return false;
    }
}

//args will passed to Commands.
//usageMsg will be shown when there is no args.
public class ArgsCommand : ICommand
{
    bool isEndTurnCommand = false;
    public bool IsEndTurnCommand => isEndTurnCommand;
    string cmdKeyWord;
    string[]? args;
    string argsUsageMsg;


    Func<string[],bool> execMethod;


    public ArgsCommand(Func<string[],bool> execMethod, string cmdKeyWord,string argsUsageMsg)
    {        
        this.execMethod = execMethod;
        this.cmdKeyWord = cmdKeyWord;
        this.argsUsageMsg = argsUsageMsg;
    }

    public void Execute()
    {
        if( args == null || string.IsNullOrWhiteSpace(args[0]) ) // if no args is given
        {
            Console.WriteLine(argsUsageMsg);
            isEndTurnCommand = false;
            return;
        }
        isEndTurnCommand = execMethod(args);        
    }
    public bool IsApplicable(string cmd, string[]? args)
    {
        if (cmd.ToLower() == cmdKeyWord)
        {
            this.args = args ;
            return true;
        }
        return false;
    }
}


public class NullOREmptyCommand : ICommand
{
    public bool IsEndTurnCommand => false;
    string cmd;

    public NullOREmptyCommand(string cmd)
    {
        this.cmd = cmd;
    }

    public void Execute()
    {
        Console.WriteLine("\tcommand not found: {0}", cmd);
        
    }

    public bool IsApplicable(string cmd, string[]? args)
    {
        return false;
    }
}