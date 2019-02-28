using System;
using UnityEngine;


public enum MoveDirection { MovePosX, MoveNegX, MovePosY, MoveNegY, MovePosZ, MoveNegZ }

namespace Assets.Scripts.Player.Commands
{

    public class Move_Command : Command
    {
        protected float moveDistance = 1.0f;
        protected MoveDirection direction;
        protected Rigidbody rgbd;
        protected Vector3 force;

        public Move_Command(Vector3 f, Rigidbody rb, MoveDirection md)
        {
            force = f;
            rgbd = rb;
            direction = md;
        }

        public override void Execute()
        {
            rgbd.AddForce(force, ForceMode.VelocityChange);

        }

        public override void UnExecute()
        {
            rgbd.AddForce(-force, ForceMode.VelocityChange);
        }

        public override string Log()
        {
            return $"{this.GetType()} has been called, force: {this.force}, direction: {direction.ToString()}";
        }
    }
}
