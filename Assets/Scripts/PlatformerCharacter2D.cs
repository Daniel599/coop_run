using System;
using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour, Character
{
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    [SerializeField] public LayerMask m_RopesToHook;
    [SerializeField] public LayerMask m_RopesToCut;
    public Side Side { get; set; }

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private RopeHook m_RopeHook;
    Collider2D m_link;
    private bool m_Paused = false;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_RopeHook = GetComponentInChildren<RopeHook>();
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        GetComponent<HingeJoint2D>().enabled = false;
    }

    public bool IsPaused()
    {
        return m_Paused;
    }

    public LayerMask RopeLayerMask { get { return m_RopesToHook; } }
    public LayerMask RopesToCut { get { return m_RopesToCut; } }

    public bool OnRope()
    {
        if (m_RopeHook == null)
        {
            return false;
        }

        return m_RopeHook.HasRope;
    }

    public bool IsGrounded()
    {
        return m_Grounded;
    }

    public void ConnectToRopeLink(Collider2D link)
    {

        Debug.Log(link.gameObject.GetComponentInParent<Chain>());
        m_AirControl = false;
        GetComponent<HingeJoint2D>().enabled = true;
        GetComponent<HingeJoint2D>().connectedBody = link.GetComponent<Rigidbody2D>();
        link.GetComponent<Rigidbody2D>().AddForce(m_Rigidbody2D.velocity * 3, ForceMode2D.Impulse);
    }




    private void FixedUpdate()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        Debug.DrawRay(m_GroundCheck.position, Vector3.down * k_GroundedRadius, Color.red);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
            }
        }
        m_Anim.SetBool("Ground", m_Grounded);

        if (m_Grounded)
        {
            m_RopeHook.ResetRopeDectection();
        }

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

#if false
        if (!OnRope())
        {
            m_link = Physics2D.OverlapCircle(m_RopeCheck.position, k_RopeRadius, m_Ropes);
            if (m_link && !m_RopeLeaveChecker.IsColliderStillAround())
            {
                ConnectToRopeLink(m_link);
            }
        }
#endif
    }


    public void Move(float move, bool crouch, bool jump)
    {
        if (m_Paused)
        {
            move = 0;
            crouch = false;
            jump = false;
        }

        if (jump)
        {
            //Debug.Log("ground:" + m_Grounded + ", y:" + m_Rigidbody2D.velocity.y);
            //Time.timeScale = 0;
        }
        if (OnRope() && jump)
        {
            GetComponent<HingeJoint2D>().enabled = false;
            m_RopeHook.LeaveRope();
            m_Rigidbody2D.AddForce(new Vector2(100f, 0));
            //m_AirControl = true;
        }

        // If crouching, check to see if the character can stand up
        if (!crouch && m_Anim.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        // Set whether or not the character is crouching in the animator
        m_Anim.SetBool("Crouch", crouch);

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move * m_CrouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.transform.position += new Vector3(0, k_GroundedRadius);
            Debug.Log("Jumping, current velocity: " + m_Rigidbody2D.velocity.ToString());
            Vector2 jump_force = new Vector2(0f, m_JumpForce);
            if (m_Rigidbody2D.velocity.x > 0 && m_Rigidbody2D.velocity.x < m_MaxSpeed)
            {
                jump_force.x = 100;
            }

            m_Rigidbody2D.AddForce(jump_force);
        }
    }


    private void Flip()
    {
        m_Rigidbody2D.velocity = new Vector2();

        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Reset()
    {
        GetComponent<HingeJoint2D>().enabled = false;
        m_RopeHook.Reset();
        //m_AirControl = true;
    }

    public void Pause()
    {
        m_Paused = true;
    }

    public void Continue()
    {
        m_Paused = false;
    }

    public void Reset(Vector3 position)
    {
        transform.position = position;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<HingeJoint2D>().enabled = false;
        m_RopeHook.Reset();
        //m_AirControl = true;
    }
}

