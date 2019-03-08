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

        public PlayerRotationCommand(Vector3 rotTow, Transform pTransform)
        {
            this.rotateTowards = rotTow;
            this.playerTransform = pTransform;
        }

        public override void Execute()
        {
            playerTransform.eulerAngles = rotateTowards;

        }

        public override string Log()
        {
            throw new NotImplementedException();
        }

        public override void UnExecute()
        {
            playerTransform.eulerAngles = -rotateTowards;

        }
    }
}
