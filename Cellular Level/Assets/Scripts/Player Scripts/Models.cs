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
<<<<<<< HEAD
<<<<<<< HEAD:Cellular Level/Assets/Scripts/Models.cs
=======
        public float RunningMultiplier;
>>>>>>> Andy-opps:Cellular Level/Assets/Scripts/Player Scripts/Models.cs
=======
<<<<<<<< HEAD:Cellular Level/Assets/Scripts/Models.cs
========
        public float RunningMultiplier;
>>>>>>>> origin/yuanwei-active2:Cellular Level/Assets/Scripts/Player Scripts/Models.cs
>>>>>>> origin/yuanwei-active2

        [Header("Jump")]
        public float JumpingHeight;
        public float JumpingFalloff;
    }

    #endregion
}
