using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.Commands
{
    class PlayerRotationCommand : Command
    {

        Vector3 rotateTowards;
        Transform playerTransform;

        public PlayerRotationCommand(Vector3 rotTow, Transform pTransform, float time)
        {
            this.rotateTowards = rotTow;
            this.playerTransform = pTransform;
            this.TimeOfExcution = time;

            this.Execute();
        }

        public override void Execute()
        {
            playerTransform.eulerAngles = new Vector3(rotateTowards.x, rotateTowards.y);

        }

        public override string Log()
        {
            return $"{this.GetType()} has been called: eulerAngles > {this.playerTransform.eulerAngles}";
        }

        public override void UnExecute()
        {
            playerTransform.eulerAngles = -rotateTowards;

        }
    }
}
