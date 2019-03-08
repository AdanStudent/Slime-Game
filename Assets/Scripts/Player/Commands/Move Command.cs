using System;
using UnityEngine;


//public enum MoveDirection { MovePosX, MoveNegX, MovePosY, MoveNegY, MovePosZ, MoveNegZ }

namespace Assets.Scripts.Player.Commands
{

    public class Move_Command : Command
    {
        protected float moveDistance = 1.0f;
        //protected MoveDirection direction;
        protected Transform playerTransform;
        protected Vector3 translation;

        public Move_Command(Vector3 f, Transform pT)
        {
            translation = f;
            playerTransform = pT;

            this.Execute();
        }

        public override void Execute()
        {
            playerTransform.Translate(translation, Space.World);
        }

        public override void UnExecute()
        {
            playerTransform.Translate(-translation, Space.World);
            Debug.Log(this.Log());

        }

        public override string Log()
        {
            return $"{this.GetType()} has been called, force: {this.translation}";
        }
    }
}
