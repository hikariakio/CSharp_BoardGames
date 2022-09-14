using System;
using System.Collections;

//ref for IEnumerable
//https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable?view=net-6.0

//Invoker for Commmand Pattern
public class CommandList  : IEnumerable
{
    private ICommand[] commands;

    public CommandList(params ICommand[] command)
    {
        this.commands = new ICommand[command.Length];

        for (int i = 0; i < command.Length; i++)
        {
            this.commands[i] = command[i];
        }
    }

    public ICommand ExtractCommand(string cmd, string[]? args = null)
    {
        for (int i = 0; i < commands.Length; i++)
        {
            if(commands[i].IsApplicable(cmd,args))
            {
                return commands[i];
            }
        }
        return new NullOREmptyCommand(cmd);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return (IEnumerator)GetEnumerator();
    }

    public CommandEnum GetEnumerator()
    {
        return new CommandEnum(commands);
    }
}

//Movenext, Reset, Current 
public class CommandEnum : IEnumerator 
{
    public ICommand[] commands;

    // Enumerators are positioned before the first element
    // until the first MoveNext() call.
    int position = -1;

    public CommandEnum(ICommand[] list)
    {
        commands = list;
    }

    public bool MoveNext()
    {
        position++;
        return (position < commands.Length);
    }

    public void Reset()
    {
        position = -1;
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public ICommand Current
    {
        get
        {
            try
            {
                return commands[position];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException();
            }
        }
    }
}