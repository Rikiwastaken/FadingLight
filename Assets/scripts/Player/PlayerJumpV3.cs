using UnityEngine;

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

    [Header("Second Jump details")]
    public bool jump2;

    [Header("Ground details")]
    [SerializeField] private Transform groundcheck;
    [SerializeField] private float radOcircle;
    [SerializeField] private float hauteurgi;
    [SerializeField] private float largeurgi;
    [SerializeField] private LayerMask whatisground;
    [SerializeField] private LayerMask whatisennemy;
    [SerializeField] private LayerMask whatisboss;
    [SerializeField] private LayerMask whatispassthrough;

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
    public bool onennemi;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator myanim;
    public float horizontal;

    public bool pressedjump = false;
    public bool presseddown = false;

    private bool alreadypressedjump;

    private PlayerMovement playermov;

    private int savepointjumpCD;

    private float velocityx;

    private Global global;
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

        global = FindAnyObjectByType<Global>();
    }

    private void Update()
    {
        if(FindAnyObjectByType<Global>().inmenu_inv_shop)
        {
            savepointjumpCD = 10;
        }
    }

    private void FixedUpdate()
    {

        int direction = 0;
        if (GetComponent<SpriteRenderer>().flipX)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        frontcheck.transform.localPosition = new Vector2(direction * Mathf.Abs(frontcheck.transform.localPosition.x), frontcheck.transform.localPosition.y);

        if ( FindAnyObjectByType<Global>().zipping && pressedjump)
        {
            FindAnyObjectByType<Global>().zipping=false;
            GetComponent<Rigidbody2D>().velocity=Vector2.zero;
        }
        if (FindAnyObjectByType<Global>().atsavepoint|| FindAnyObjectByType<Global>().indialogue || FindAnyObjectByType<Global>().zipping || (FindAnyObjectByType<Global>().grappling && !GetComponent<GrappleScript>().grapplingenemy)|| FindAnyObjectByType<Global>().inpause)
        {
            savepointjumpCD=(int)(1/Time.deltaTime);
            stuckinwall = false;
            wantstounstick = false;
            
            GetComponent<Animator>().SetBool("stuckinwall", false);
            return;
        }


        if(savepointjumpCD>0)
        {
            savepointjumpCD--;
        }
        if (alreadypressedjump && !pressedjump)
        {
            alreadypressedjump = false;
        }
        HandleLayers();


        horizontal = playermov.horizontal;

        if(!grounded && (Physics2D.OverlapCircle(groundcheck.position, radOcircle, whatisground)))// here to get the frame where the player is grounded again
        {
            rb.velocity = new Vector2(velocityx,0);
        }

        grounded = (Physics2D.OverlapCircle(groundcheck.position, radOcircle, whatisground) || Physics2D.OverlapBox(groundcheck.position, new Vector2(largeurgi/2f, hauteurgi), 0, whatispassthrough));
        onennemi = (Physics2D.OverlapCircle(groundcheck.position, radOcircle, whatisennemy) || Physics2D.OverlapCircle(groundcheck.position, radOcircle, whatisboss));
        touchingwall = Physics2D.OverlapBox(frontcheck.position, new Vector2(hauteurgi/2, largeurgi/2), 0, whatiswall) && global.worldflags[11];
        Checkground();

        //normal jump

        if (pressedjump && grounded && !alreadypressedjump && savepointjumpCD<=0 && GetComponent<PlayerDodge>().airdodgelengthcnt == 0)
        {
            alreadypressedjump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            myanim.SetTrigger("jump");
        }
        if (!grounded && alreadypressedjump && jumpcounter > 0 && savepointjumpCD <= 0 && GetComponent<PlayerDodge>().airdodgelengthcnt == 0)
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

        //doublejump

        if (pressedjump && !grounded && !alreadypressedjump && !jump2 &&!stuckinwall && GetComponent<PlayerDodge>().airdodgelengthcnt == 0 && global.worldflags[12])
        {
            
            alreadypressedjump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce*1.5f);
            myanim.SetTrigger("jump");
            jump2 = true;

            // Reset AirDash
            GetComponent<PlayerDodge>().resetairdash = true;
        }

        //wall jump
        if (!touchingwall)
        {
            stuckinwall = false;
            wantstounstick = false;
            rb.gravityScale = gravity;
            GetComponent<Animator>().SetBool("stuckinwall", false);
        }
        else
        {
            if (presseddown)
            {
                wantstounstick = true;
            }
            
            if (!wantstounstick && ((direction==1 && horizontal>0)||(direction == -1 && horizontal < 0)))
            {
                stuckinwall = true;
            }
            else
            {
                stuckinwall = false;
                rb.gravityScale = gravity;
                GetComponent<Animator>().SetBool("stuckinwall", false);
            }
        }

        if (stuckinwall)
        {
            jump2 = false;
            GetComponent<Animator>().SetBool("stuckinwall", true);
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0;
            if (pressedjump && !alreadypressedjump)
            {
                alreadypressedjump = true;
                rb.velocity = new Vector2(-direction * wjforceside, wjforceup);
                rb.gravityScale = gravity;
                stuckinwall = false;
                GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
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

        velocityx = rb.velocityX;

    }

    void Checkground()
    {
        if (grounded)
        {
            if(!alreadypressedjump)
            {
                jumpcounter = jumptime;
            }
            myanim.ResetTrigger("jump");
            myanim.SetBool("falling", false);
            touchingwall = false;
            jump2 = false;
        }
        if(onennemi)
        {
            if (!alreadypressedjump)
            {
                jumpcounter = jumptime;
            }
            myanim.ResetTrigger("jump");
            myanim.SetBool("falling", false);
            touchingwall = false;
            jump2 = false;
        }
    }

    private void OnDrawGizmos()
    {

        //Gizmos.DrawCube(groundcheck.position - new Vector3(0, 0.1f, 0), new Vector2(largeurgi, hauteurgi));
        Gizmos.DrawCube(frontcheck.position, new Vector2(hauteurgi / 2, largeurgi / 2));

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
