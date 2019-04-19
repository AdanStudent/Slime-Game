//main class where other Commands can inherit from

public abstract class Command
{
 
    //getting the transform component from the command
    public UnityEngine.Transform GetTransform { get; set; }

    //saving the Time at which the command is created
    public float TimeOfExcution { get; protected set; }

    //used to Execute action associated with it's command
    public abstract void Execute();
    
    //used to Undo the action associated with it's command
    public abstract void UnExecute();
    
    //Logs which command has run
    public abstract string Log();
}
