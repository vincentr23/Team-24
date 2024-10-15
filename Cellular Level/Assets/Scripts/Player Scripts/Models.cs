using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Models
{
    #region #region - Player -
    [System.Serializable]
    public class PlayerSettingsModel
    {
        [Header("View Settings")]
        public float ViewXSens;
        public float ViewYSens;

        [Header("Movement")]
        public float WalkingForwardSpeed;
        public float WalkingBackwardSpeed;
        public float WalkingStrafeSpeed;
        public float RunningForwardSpeed;

        [Header("Jump")]
        public float JumpingHeight;
        public float JumpingFalloff;
    }

    #endregion
}
