using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public Animator anim;
    public float speed = 6;
    public float jumpForce = 500;
    public bool canDoubleJump = false;
    public bool canDash = false;
    public float dashSpeed = 15;
    public float dashDuration = 0.5f;
    public bool canAirDash = false;

    private Rigidbody2D rb;
    private float dirX;
    private bool isGrounded = false;
    private bool moving = false;
    private bool airMove = false;
    private bool onDashing = false;
    private float originalGravity = 1;
    private float moveHorizontal;
    public Transform dropItemPrefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        rb = GetComponent<Rigidbody2D>();
        //rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (!anim && GetComponent<Animator>())
        {
            anim = GetComponent<Animator>();
        }
    }
    void Update()
    {
        UpdateIsGrounded();

        if (Time.timeScale == 0.0f)
        {
            if (onDashing)
            {
                CancelDash();
            }
            if (anim)
            {
                anim.SetBool("run", false);
            }
            //rb.velocity = Vector2.zero;
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }
        if (onDashing)
        {
            return;
        }

        if (canDash && Input.GetKeyDown(KeyCode.Mouse1) && isGrounded)
        {
            StartCoroutine("Dash");
        }
        if (canAirDash && Input.GetKeyDown(KeyCode.Mouse1) && !isGrounded && !airMove)
        {
            StartCoroutine("AirDash");
        }
        moveHorizontal = Input.GetAxis("Horizontal");
        //Flip Right Side
        if (moveHorizontal > 0.1)
        {
            Vector3 rot = transform.eulerAngles;
            rot.y = 0;
            transform.eulerAngles = rot;
        }
        //Flip Left Side
        if (moveHorizontal < -0.1)
        {
            Vector3 rot = transform.eulerAngles;
            rot.y = 180;
            transform.eulerAngles = rot;
        }
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0.0f)
        {
            moveHorizontal = 0;
            return;
        }
        if (onDashing)
        {
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                CancelDash();
            }
            Vector3 dir = transform.TransformDirection(Vector3.right);
            GetComponent<Rigidbody2D>().velocity = dir * dashSpeed;
            return;
        }

        dirX = moveHorizontal * speed;

        rb.velocity = new Vector2(dirX, rb.velocity.y);

        if (moveHorizontal != 0)
        {
            moving = true;
            if (anim)
            {
                anim.SetBool("run", moving);
            }
        }
        else if (moving)
        {
            moving = false;
            if (anim)
            {
                anim.SetBool("run", moving);
            }
        }
        if (Input.GetButton("Jump") && isGrounded)
        {
            anim.SetTrigger("jump");
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce);
        }
        if (canDoubleJump && !airMove || !airMove)
        {
            if (Input.GetButtonDown("Jump") && !isGrounded)
            {
                DoubleJump();
            }
        }
    }

    public void DashButton()
    {
        if (onDashing)
        {
            return;
        }
        if (canDash && isGrounded)
        {
            StartCoroutine("Dash");
        }
        if (canAirDash && !isGrounded && !airMove)
        {
            StartCoroutine("AirDash");
        }
    }

    public void JumpButton()
    {
        if (isGrounded)
        {
            anim.SetTrigger("jump");
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce);
        }

        if (canDoubleJump && !airMove && !airMove)
        {
            if (!isGrounded)
            {
                DoubleJump();
            }
        }
    }

    void DoubleJump()
    {
        anim.SetTrigger("jump");
        rb.velocity = new Vector2(rb.velocity.x, 0.1f);
        rb.AddForce(Vector2.up * jumpForce);
        airMove = true;
    }

    void UpdateIsGrounded()
    {
        if (airMove && onDashing)
        {
            return;
        }

        if (rb.velocity.y == 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        anim.SetBool("grounded", isGrounded);
        if (airMove && isGrounded)
        {
            airMove = false;
        }
    }

    public IEnumerator Dash()
    {
        if (!onDashing)
        {
            onDashing = true;
            anim.SetTrigger("dash");
            anim.ResetTrigger("cancelDash");
            yield return new WaitForSeconds(dashDuration);
            CancelDash();
        }
    }

    public void CancelDash()
    {
        StopCoroutine("Dash");
        anim.SetTrigger("cancelDash");
        rb.gravityScale = originalGravity;
        onDashing = false;
    }

    public IEnumerator AirDash()
    {
        if (!onDashing && !airMove)
        {
            airMove = true;
            onDashing = true;
            rb.gravityScale = 0.01f;
            anim.SetTrigger("dash");
            anim.ResetTrigger("cancelDash");
            yield return new WaitForSeconds(dashDuration);
            rb.gravityScale = originalGravity;
            CancelDash();
        }
    }

}
