using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.Commands
{
    class CameraRotationCommand : Command
    {
        //keeps the player's current rotation
        protected Vector3 currentRotation;

        //keeps the player's current position
        protected Vector3 currentPosition;
        
        //Distance of camera from Target
        protected float distFromTarget;

        //transform of the Target(empty gameObject) that's attached to
        //the player
        protected Transform camTarget;

        protected Transform camTransform;


        public CameraRotationCommand(Vector3 currRot, Vector3 currPos, float dist, Transform camTar, Transform camTrans)
        {
            this.currentRotation = currRot;
            this.currentPosition = currPos;
            this.distFromTarget = dist;
            this.camTarget = camTar;
            this.camTransform = camTrans;

            this.Execute();
        }

        public override void Execute()
        {
            camTransform.eulerAngles = currentRotation;
            camTransform.position = camTarget.position - camTransform.forward * distFromTarget;
        }

        public override string Log()
        {
            return $"{this.GetType()} has been called, current Rotation: {this.currentRotation}, current Position: {this.currentPosition}," +
                $" camTarget: {this.camTarget.position}, camTransform Pos: {this.camTransform.position}, camTransform Angles: {this.camTransform.eulerAngles}";
        }

        public override void UnExecute()
        {
            camTransform.eulerAngles = -currentRotation;
            camTransform.position = camTarget.position + camTransform.forward * distFromTarget;
        }
    }
}
