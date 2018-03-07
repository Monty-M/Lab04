using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Rigidbody2D rigidBody;
    public float speed;
    public float jumpForce;
    Animator anim;
    public bool isGrounded;
    public LayerMask isGroundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;
    public bool isFacingLeft;

    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (!rigidBody)
        {
            Debug.LogError("No RigidBody2D attached to " + name);
        }

        if (!anim)
        {
            Debug.LogError("No Animator attached to " + name);
        }

        if (speed <= 0)
        {
            speed = 3.0f;
            Debug.LogWarning("Speed not set. Defaulting to " + speed);
        }

        if (jumpForce <= 0)
        {
            jumpForce = 3.0f;
            Debug.LogWarning("JumpForce not set. Defaulting to " + jumpForce);
        }

        if (!groundCheck)
        {
            Debug.LogError("No GroundCheck attached to " + name);
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.1f;
            Debug.LogWarning("GroundCheckRadius not set. Defaulting to " + groundCheckRadius);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        float moveValue = Input.GetAxisRaw("Horizontal"); //Input.GetAxisRaw("Horizontal") slight visual accelaration
        float jumpValue = Input.GetAxisRaw("Vertical");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            Debug.Log("Jump");
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetButton("Jump") && Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("Attack!");
            rigidBody.AddForce(Vector2.down * jumpForce, ForceMode2D.Impulse);
        }

        rigidBody.velocity = new Vector2(moveValue * speed, rigidBody.velocity.y);

        anim.SetFloat("MoveValue", Mathf.Abs(moveValue));
        anim.SetFloat("JumpValue", Mathf.Abs(jumpValue));
        anim.SetBool("Grounded", isGrounded);

        if ((!isFacingLeft && moveValue < 0) || (isFacingLeft && moveValue > 0))
            flip();
    }

    void flip()
    {
        isFacingLeft = !isFacingLeft;
        Vector3 scaleFactor = transform.localScale;
        scaleFactor.x = -scaleFactor.x;
        transform.localScale = scaleFactor;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered with " + collision.gameObject.name);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
        if (collision.gameObject.name == "Collectible_Coin")
        {
            Debug.Log("Destroyed " + collision.gameObject);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "Block")
        {
            Debug.Log("Colliding with " + collision.gameObject.name);
            Destroy(collision.gameObject);
        }
    }

    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Block")
        {
            Debug.Log("Colliding with " + collision.gameObject.name);
            Destroy(collision.gameObject);
        }
    }
    */

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Block_Hit")
        {
            Debug.Log("Collided with " + collision.gameObject.name);
            Destroy(collision.gameObject);
        }
    }
}

/*
public class Teleport : MonoBehaviour
{

    public GameObject Pipe_1_Teleport;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Mario")
        {
            transform.position = new Vector3(-19, -2, 0);//(where you want to teleport)
        }
    }
}
*/