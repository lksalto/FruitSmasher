using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{
    public float fallMultiplier = 3.4f;
    public float lowJumpMultiplier = 2f;
    public float goDown = 5f;
    Rigidbody2D rb;
    [SerializeField] PlayerMovement player;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.W))
        {
            //rb.velocity = new Vector2(0, 0);    //pulo mais controlado
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
            {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (goDown - 1) * Time.deltaTime;
        }

    }
}
