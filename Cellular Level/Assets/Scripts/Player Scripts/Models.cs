using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Models
{
    #region - Player -
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
    }

    #endregion
}
