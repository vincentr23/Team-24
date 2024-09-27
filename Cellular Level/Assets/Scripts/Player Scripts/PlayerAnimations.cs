using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimation : MonoBehaviour
{

    Animator anim;
    PlayerController playerController;
    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Awake()
    {   
        playerController = player.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Running", playerController.IsRunning());
        anim.SetFloat("Vertical", playerController.GetYVel());
        anim.SetFloat("Horizontal", playerController.GetXVel());
        if (playerController.GetInput().Player.Fire.IsPressed())
           anim.SetTrigger("Gather");
        else anim.ResetTrigger("Gather");

    }
}