using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBowController : MonoBehaviour
{
    public PlayerController _controller;
    private PlayerController P_Controller => _controller;

    public Animator animator;

    void Start()
    {
        // Transform currentTransform = transform;
        // while (currentTransform.parent != null)
        // {
        //     currentTransform = currentTransform.parent;
        // }
        // _controller = currentTransform.GetComponent<PlayerController>();
        _controller = GameManager.instance.gameData.player.GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (P_Controller.returnIsAim())
        {
            animator.SetBool("isAim", true);
        }
        else
        {
            animator.SetBool("isAim", false);
        }
    }
}
