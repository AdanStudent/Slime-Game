//main class where other Commands can inherit from
public abstract class Command
{
    //used to Execute action associated with it's command
    public abstract void Execute();
    
    //used to Undo the action associated with it's command
    public abstract void UnExecute();
    
    //Logs which command has run
    public abstract string Log();

}
