using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    PlayerMovement player;
    Animator anim;
    public bool isHoldingDown;
    public bool canAttack;
    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
        anim = GetComponentInParent<Animator>();
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAnimation();
    }

    void HandleAnimation()
    {
        isHoldingDown = Input.GetKey(KeyCode.S);
        anim.SetBool("isWalking", player.horizontalInput != 0);
        if(canAttack)
        {
        
            if (player.isGrounded && Mathf.Approximately(player.rb.velocity.y, 0))
            {   if(isHoldingDown)
                {
                    if (player.horizontalInput != 0)
                    {
                        if (Input.GetKey(KeyCode.Mouse1))
                        {
                            Invoke("PerformAtkFHW", player.inputBufferTime);
                        }

                    }
                    else if (Input.GetKey(KeyCode.Mouse1))
                    {

                        Invoke("PerformAtkFHD", player.inputBufferTime);
                        
                    }
                }
                else
                {
                    if(player.horizontalInput != 0)
                    {
                        if(Input.GetKey(KeyCode.Mouse1))
                        {
                            Invoke("PerformAtkFHW", player.inputBufferTime);
                        }
                        
                    }
                    else if (Input.GetKey(KeyCode.Mouse1))
                    {
                        Invoke("PerformAtkFHS", player.inputBufferTime);
                    }
                }


            }
        }
    }
    void StopPlayer()
    {
        player.SetCanMove(false);
        player.rb.velocity = Vector3.zero;
        canAttack = false;
    }
    void CanMove()
    {
        player.SetCanMove(true);
        canAttack = true;
    }

    void PerformAtkFHS()
    {
        if (!anim.GetBool("isWalking"))
        {
            anim.SetTrigger("atkFHS");
        }
    }
    void PerformAtkFHD()
    {
        if (!anim.GetBool("isWalking"))
        {
            anim.SetTrigger("atkFHD");
        }

    }

    void PerformAtkFHW()
    {
        if (anim.GetBool("isWalking"))
        {
            anim.SetTrigger("atkFHW");
        }

    }

    void PushPlayer()
    {
        Debug.Log("PUSH");
        if(player.sr.flipX)
        {
            player.rb.AddForce(new Vector3(-9, 0, 0), ForceMode2D.Impulse);
        }
        else
        {
            player.rb.AddForce(new Vector3(9, 0, 0), ForceMode2D.Impulse);
        }
    }

}
