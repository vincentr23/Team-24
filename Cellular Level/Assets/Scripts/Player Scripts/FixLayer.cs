using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixLayer : MonoBehaviour
{
    private PlayerController player;
    // Start is called before the first frame update

    private void Start()
    {
        if (gameObject.layer == 0)
        {
            player = GetComponentInParent<PlayerController>();
            var m_player = player.PlayerNum();
            var m_layer = player.DecipherLayer(m_player);
            gameObject.layer = LayerMask.NameToLayer(m_layer);
        }
    }
}
