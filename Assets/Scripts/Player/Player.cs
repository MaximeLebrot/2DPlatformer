using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public float jumpHeight = 0.5f;
    //Time to reach top of jump
    public float timeToJumpApex = .35f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .07f;
    public float moveSpeed = 1.5f;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;
    float velocityYSmoothing;

    public Vector2 directionalInput;

    CharacterController2D controller2D;
    Animator anim;
    AnimationController animationController;
    PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        controller2D = GetComponent<CharacterController2D>();
        anim = GetComponent<Animator>();
        animationController = GetComponent<AnimationController>();
        playerStats = GetComponent<PlayerStats>();

        //Calculate gravity and jump velocity
        gravity = -((2*jumpHeight)/Mathf.Pow(timeToJumpApex,2));
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ComputeVelocity();
        SpikeDetection();
        EnemyDetection();

        controller2D.Move(velocity * Time.deltaTime, directionalInput);     

        //Let animationController script handle animation
        animationController.AnimateCharacter(directionalInput);

        //Reset velocity when it hits the ground
        if (controller2D.collisions.above || controller2D.collisions.below)
        {
                velocity.y = 0;
        }
        
    }

    public bool canDoubleJump;
    int jumpsLeft = 0;

    public void Jump()
    {
        //If the character is touching the ground and nothing is over it
        if (controller2D.collisions.below)
        {
            anim.SetBool("jumping", true);
            velocity.y = jumpVelocity;
            jumpsLeft = 1;
        }
        else if (canDoubleJump && jumpsLeft > 0)
        {
            jumpsLeft--;
            anim.SetBool("jumping", true);
            velocity.y = jumpVelocity;
        }

    }

    //For respawning
    public void ResetVelocity()
    {
        velocity.y = 0;
        velocity.x = 0;
    }

    public void SpikeDetection()
    {
        if (controller2D.collisions.left || controller2D.collisions.right )
        {
            if (controller2D.collisions.hitInfoHorizontal.transform.tag == "Spike")
            {
                if (!playerStats.Dead)
                { 
                    //Kill player
                    playerStats.Die();
                }          
            }
        }
        if(controller2D.collisions.below || controller2D.collisions.above)
        {
            if (controller2D.collisions.hitInfoVertical.transform.tag == "Spike")
            {
                if (!playerStats.Dead)
                {
                    //Kill player
                    playerStats.Die();
                }
            }
        }

    }
    public void EnemyDetection()
    {
        if (controller2D.collisions.below)
        {
            //Hit enemy from top
            if (controller2D.collisions.hitInfoVertical.transform.tag == "Enemy")
            {
                //Damage enemy        

                controller2D.collisions.hitInfoVertical.transform.gameObject.GetComponent<Enemy>().TakeDamage(playerStats.BaseDamage * playerStats.DamageMultiplier);
                KnockBack(new Vector2(0, 10f), true, false);
            }
            else if (controller2D.collisions.hitInfoVertical.transform.tag == "Boss")
            {
                controller2D.collisions.hitInfoVertical.transform.gameObject.GetComponent<Enemy>().TakeDamage(playerStats.BaseDamage * playerStats.DamageMultiplier);
                KnockBack(new Vector2(0, 12f), true, false);
            }
        }

        if (controller2D.collisions.right)
        {
            //Hit enemy from top
            if (controller2D.collisions.hitInfoHorizontal.transform.tag == "Enemy")
            {
                playerStats.TakeDamage(controller2D.collisions.hitInfoHorizontal.transform.gameObject.GetComponent<Enemy>().damageAmmount);
                KnockBack(new Vector2(-3f, 6f), true, true);
            }
            else if (controller2D.collisions.hitInfoHorizontal.transform.tag == "Boss")
            {
                playerStats.TakeDamage(controller2D.collisions.hitInfoHorizontal.transform.gameObject.GetComponent<Enemy>().damageAmmount);
                KnockBack(new Vector2(-5f, 10f), true, true);
            }
        }
        if (controller2D.collisions.left)
        {
            //Hit enemy from top
            if (controller2D.collisions.hitInfoHorizontal.transform.tag == "Enemy")
            {
                playerStats.TakeDamage(controller2D.collisions.hitInfoHorizontal.transform.gameObject.GetComponent<Enemy>().damageAmmount);
                KnockBack(new Vector2(3f, 6f), true, true);
            }
            else if (controller2D.collisions.hitInfoHorizontal.transform.tag == "Boss")
            {
                playerStats.TakeDamage(controller2D.collisions.hitInfoHorizontal.transform.gameObject.GetComponent<Enemy>().damageAmmount);
                KnockBack(new Vector2(5f, 10f), true, true);
            }
        }
    }

    bool loseControl;
    float currentKnockBack; 

    IEnumerator KnockBackTimer()
    {
        loseControl = true;
        yield return new WaitForSeconds(.3f);
        loseControl = false;
        currentKnockBack = 0;
    }

    public void KnockBack(Vector2 knockBack, bool changeYVelocity,bool changeXVelocity)
    {
        velocity.x = 0;
        velocity.y = 0;
        if (changeYVelocity)
        {
            velocity.y = knockBack.y;
        }
        if (changeXVelocity)
        {
            StartCoroutine(KnockBackTimer());
            velocity.x = knockBack.x;
            currentKnockBack = knockBack.x;
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    void ComputeVelocity()
    {
        float targetVelocityX;

        if (loseControl)
        {
            targetVelocityX = currentKnockBack;
        }
        else
        {
            targetVelocityX = directionalInput.x * moveSpeed;
        }
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller2D.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }
}
