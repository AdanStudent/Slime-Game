using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Command : Command
{
    float Damage;
    GameObject Target;

    // The player would need HP somewhere
    // In case an Attack Damage number isn't provided, 10 is a default value
    // A Target is Required, however
    // Edits can be made to adjust it to the parameters we'll actually use
    public Attack_Command(GameObject TargetObject) : this(TargetObject, 10) { }

    public Attack_Command(GameObject TargetObject, float AttackDamage)
    {
        this.Damage = AttackDamage;
        this.Target = TargetObject;

        this.Execute();
    }

    public override void Execute()
    {
        // Have the player lose HP based on the Damage
        // In my testing, I just threw an HP value into InputHandler
        Target.GetComponent<InputHandler>().HealthPoints -= Damage;
    }

    public override void UnExecute()
    {
        // Have the player gain HP based on the Damage
        // In my testing, I just threw an HP value into InputHandler
        Target.GetComponent<InputHandler>().HealthPoints += Damage;
    }

    public override string Log()
    {
        return $"{this.GetType()} has been called, target : { this.Target} has taken {this.Damage} points of damage";
    }

}
