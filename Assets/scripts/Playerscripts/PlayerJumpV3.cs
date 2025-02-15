using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]


public class PlayerJumpV3 : MonoBehaviour
{
    PlayerControls controls;

    [Header("hitbox of the attack")]
    public Transform attackpoint;
    public float range;

    [Header("Jump details")]
    public float jumpForce;
    public float jumptime;
    public float jumpcounter;
    public bool allowjump;

    [Header("Ground details")]
    [SerializeField] private Transform groundcheck;
    [SerializeField] private float radOcircle;
    [SerializeField] private float hauteurgi;
    [SerializeField] private float largeurgi;
    [SerializeField] private LayerMask whatisground;

    [Header("Walljump details")]
    public bool touchingwall; //checks if in contact with wall
    public float gravity;
    public float wjforceside;
    public float wjforceup;
    public bool stuckinwall; // is true if player is sticking to a wall
    private bool wantstounstick; //check if player wants to unstick from wall
    [SerializeField] private LayerMask whatiswall;
    [SerializeField] private Transform frontcheck;

    public bool grounded;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator myanim;
    public float horizontal;

    public bool pressedjump = false;
    public bool presseddown = false;

    private bool alreadypressedjump;

    private PlayerMovement playermov;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.gameplay.jump.performed += ctx => pressedjump = true;
        controls.gameplay.jump.canceled += ctx => pressedjump = false;
        controls.gameplay.down.performed += ctx => presseddown = true;
        controls.gameplay.down.canceled += ctx => presseddown = false;
        rb = GetComponent<Rigidbody2D>();
        jumpcounter = jumptime;
        myanim = GetComponent<Animator>();

        gravity = rb.gravityScale;

        playermov = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        if(alreadypressedjump && !pressedjump)
        {
            alreadypressedjump = false;
        }
        HandleLayers();


        horizontal = playermov.horizontal;
        grounded = Physics2D.OverlapCircle(groundcheck.position, radOcircle, whatisground);
        touchingwall = Physics2D.OverlapBox(frontcheck.position, new Vector2(hauteurgi, largeurgi),0, whatiswall);
        Checkground();

        //normal jump

        if (pressedjump && grounded && !alreadypressedjump)
        {
            alreadypressedjump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            myanim.SetTrigger("jump");
        }
        if (!grounded && pressedjump && jumpcounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpcounter -= Time.deltaTime;
            myanim.SetTrigger("jump");
        }
        if (!pressedjump && !grounded)
        {
            jumpcounter = 0;
            myanim.SetBool("falling", true);
            myanim.SetTrigger("jump");
        }
        if (rb.velocity.y < 0)
        {
            myanim.SetBool("falling", true);
        }

        //wall jump

        if(!touchingwall)
        {
            stuckinwall = false;
            wantstounstick = false;
            rb.gravityScale = gravity;
        }
        else
        {
            if(presseddown)
            {
                wantstounstick = true;
            }
            if(!wantstounstick)
            {
                stuckinwall = true;
            }
            else
            {
                stuckinwall = false;
                rb.gravityScale = gravity;
            }
        }

        if (stuckinwall)
        {
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0;
            if (pressedjump && !alreadypressedjump)
            {
                alreadypressedjump = true;
                rb.velocity = new Vector2(-transform.localScale.x/Mathf.Abs(transform.localScale.x) * wjforceside, wjforceup);
                rb.gravityScale = gravity;
                stuckinwall = false;
                Vector3 Scale = transform.localScale;
                Scale.x *= -1;
                transform.localScale = Scale;
                if (playermov.facingRight)
                {
                    playermov.facingRight = false;
                }
                else
                {
                    playermov.facingRight = true;
                }
            }
        }
    }

    void Checkground()
    {
        if (grounded)
        {
            allowjump = true;
            jumpcounter = jumptime;
            myanim.ResetTrigger("jump");
            myanim.SetBool("falling", false);
            touchingwall = false;

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundcheck.position, new Vector2(largeurgi, hauteurgi));
        Gizmos.DrawCube(frontcheck.position, new Vector2(hauteurgi, largeurgi));

    }

    private void HandleLayers()
    {
        if (!grounded)
        {
            myanim.SetLayerWeight(1, 1);
        }
        else
        {
            myanim.SetLayerWeight(1, 0);
        }
    }
    void OnEnable()
    {
        controls.gameplay.Enable();
    }
    void OnDisable()
    {
        controls.gameplay.Disable();
    }
}
