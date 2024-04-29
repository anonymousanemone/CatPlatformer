using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    //jump tutorial: https://www.youtube.com/watch?v=K1xZ-rycYY8&ab_channel=bendux
    //ladder tutorial: https://www.youtube.com/watch?v=Ln7nv-Y2tf4&t=45s&ab_channel=Blackthornprod

    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float speed;
    [SerializeField] private float jumpingPower = 16f;

    private bool grounded;
    [SerializeField]private Transform groundCheck;
    [SerializeField]private LayerMask groundLayer;
    private bool climbing;
    [SerializeField]private LayerMask ladderMask;
    [SerializeField]private float distance;
    [SerializeField]private CoinManager cm;

    private float horizontalInput;
    private float verticalInput;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        //move player
        horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        Climb();
        Jump();
        Flip();

        //set animator parameters
        if (horizontalInput == 0) {
            anim.SetFloat("sit-time", anim.GetFloat("sit-time") + 0.01f);
        }
        else
        {
            anim.SetFloat("sit-time", 0);
        }
        anim.SetBool("walk", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
        anim.SetBool("climb", climbing);


    }

    private void Climb()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, ladderMask);

        if(hitInfo.collider != null)
        {
            //Debug.Log(climbing);
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                climbing = true;
               
            }
        }
        else
        {
            climbing = false;
        }
        //Debug.Log(verticalInput);
        if (climbing == true && hitInfo.collider !=null)
        {
            verticalInput = Input.GetAxisRaw("Vertical");
            //Debug.Log(verticalInput);
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * speed);
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 5;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            anim.SetTrigger("jump");
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            cm.coinCount++;
        }
        
    }

}
