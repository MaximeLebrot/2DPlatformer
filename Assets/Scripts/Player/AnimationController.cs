using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController2D))]
public class AnimationController : MonoBehaviour
{
    Animator anim;
    CharacterController2D controller2D;
    PlayerStats playerStats;

    private void Start()
    {
        anim = GetComponent<Animator>();
        controller2D = GetComponent<CharacterController2D>();
        playerStats = GetComponent<PlayerStats>();
    }
    public void AnimateCharacter(Vector2 directionalInput)
    {

        anim.SetBool("dead",playerStats.Dead);
        anim.SetBool("takingDamage",playerStats.TakingDamage);
        anim.SetBool("isGrounded", controller2D.collisions.below);
        if (directionalInput.x != 0)
        {
            anim.SetBool("moving", true);
            //Set run animation speed according to input
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Run"))
            {
                anim.speed = 1f * Mathf.Abs(directionalInput.x) * 1.2f; 
            }
        }
        else
        {
            anim.speed = 1f;
            anim.SetBool("moving", false);
        }

        if (controller2D.collisions.below)
        {
            anim.SetBool("jumping", false);
        }

        
    }
    
}
