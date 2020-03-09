using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float dashDistance = 3;

    [SerializeField] private float m_MaxSpeed = 10f;                            // Amount of max speed added when the player runs.
    [SerializeField] private float m_RunPower = 6000f;                          // Amount of speed added when the player runs.
    [SerializeField] private float m_JumpPowerInitial = 10f;                    // Amount of speed added when the player initiate jumps.
    [SerializeField] private float m_JumpPower = 10f;                           // Amount of speed added when the player jumps.
    [SerializeField] private float m_WallJumpPower = 10f;                       // Amount of speed added when the player wall jumps.
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] public Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] public Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] public Transform[] m_WallCheck;                             // A position marking where to check for walls
    [SerializeField] public Transform m_PlayerCenter;                            

    public bool m_Attacking;
    const float k_GroundedRadius = .05f; // Radius of the overlap circle to determine if grounded
    public bool m_Grounded;            // Whether or not the player is grounded.
    private bool m_WallClimbed;         // Whether or not the player is wall climbed.
    public bool m_Controllable = true;
    const float k_CeilingRadius = .2f;  // Radius of the overlap circle to determine if the player can stand up
    const float k_WallRadius = .05f;     // Radius of the overlap circle to determine if the player wall jump
    const int jumpTime = 20;
    private Rigidbody2D m_Rigidbody2D;
    public bool m_FacingRight = true;  // For determining which way the player is currently facing.
    public bool m_Jumping = false;
    private Coroutine wallJumpCoroutine = null;

    private int jumpTimeCounter;


    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }


    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }
    
    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded && !m_Attacking)
                {
                    OnLandEvent.Invoke();
                }
            }
        }

        m_WallClimbed = false;
        if (!m_Grounded && !m_Attacking)
        {
            bool wallClimbChecker = true;
            foreach(Transform child in m_WallCheck)
            {
                bool tempWallChecker = false;
                Collider2D[] wallColliders;
                wallColliders = Physics2D.OverlapCircleAll(child.position, k_WallRadius, m_WhatIsGround);

                for (int i = 0; i < wallColliders.Length; i++)
                {
                    if (wallColliders[i].gameObject != gameObject && (Input.GetAxisRaw("Horizontal") != 0) && !m_Attacking)
                    {
                        tempWallChecker = true;
                        break;
                    }
                }
                wallClimbChecker &= tempWallChecker;
            }

            if (wallClimbChecker)
            {
                animator.SetTrigger("WallClimb");
                m_WallClimbed = true;
                m_Rigidbody2D.gravityScale = 0;
                m_Rigidbody2D.velocity = Vector2.zero;
            }

            if (!m_WallClimbed)
            {
                m_Rigidbody2D.gravityScale = 10;
                if(m_Rigidbody2D.velocity.y < 0 && !m_Attacking)
                {
                    animator.SetBool("JumpDown", true);
                }
            }
        }
    }
    public void OnLand()
    {
        animator.SetTrigger("Land");
        animator.SetBool("JumpDown", false);
    }

    public void Move(float move)
    {
        if (m_Controllable && !m_Attacking)
        {
            animator.SetBool("Run", move != 0 ? true : false);
            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                if (m_Rigidbody2D.velocity.magnitude < m_MaxSpeed || (!m_Grounded && (move > 0 ^ m_Rigidbody2D.velocity.x > 0)))
                {
                    m_Rigidbody2D.AddForce(new Vector2(move * m_RunPower, 0));
                }

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
                //Enforce friction when there is no input yet player moving
                if (m_Grounded && move == 0 && m_Rigidbody2D.velocity.magnitude > 0)
                {
                    m_Rigidbody2D.velocity = new Vector2(Mathf.Lerp(m_Rigidbody2D.velocity.x, 0, 0.5f), m_Rigidbody2D.velocity.y);
                }
            }
        }
    }

    public void Jump(bool jumpKeyDown, bool jumpKey, bool jumpKeyUp)
    {
        if (m_Controllable && !m_Attacking)
        {
            if (jumpKeyDown)
            {
                if (m_Grounded)
                {
                    m_Jumping = true;
                    jumpTimeCounter = jumpTime;
                    animator.SetTrigger("Jump");
                    m_Rigidbody2D.AddForce(new Vector2(0, m_JumpPowerInitial));
                }
                if (m_WallClimbed)
                {
                    m_WallClimbed = false;

                    if (wallJumpCoroutine != null) StopCoroutine(wallJumpCoroutine);

                    m_Rigidbody2D.AddForce(new Vector2(m_WallJumpPower * (m_FacingRight ? -1 : 1), m_WallJumpPower));
                    wallJumpCoroutine = StartCoroutine(WallJump());
                    animator.SetTrigger("Jump");

                    Flip();
                }
            }
            if (jumpKey && m_Jumping)
            {
                if (jumpTimeCounter > jumpTime / 2)
                {
                    //m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpSpeed / 1.5f);
                    m_Rigidbody2D.AddForce(new Vector2(0, m_JumpPower));
                    jumpTimeCounter -= 1;
                }
                else if (jumpTimeCounter > 0)
                {
                    //m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpSpeed);
                    m_Rigidbody2D.AddForce(new Vector2(0, m_JumpPower));
                    jumpTimeCounter -= 1;
                }
                else
                {
                    m_Jumping = false;
                }
            }
            if (jumpKeyUp || m_WallClimbed)
            {
                m_Jumping = false;
            }
        }
    }

    private IEnumerator WallJump()
    {
        float multiplifier = 0.1f;
        m_AirControl = false;
        for (float i = 0; i < 0.1f; i += Time.deltaTime)
        {
            yield return null;
            m_Rigidbody2D.AddForce(new Vector2(0, m_WallJumpPower * 1.5f) * multiplifier);
        }
        m_AirControl = true;
    }

    public IEnumerator Dash(DashDir _dir, Vector2 dashZonePos)
    {
        m_Controllable = false;
        Vector2 dir = Vector2.zero;
        switch (_dir)
        {
            case DashDir.Up: dir = Vector2.up; break;
            case DashDir.Down: dir = Vector2.down; break;
            case DashDir.Left: dir = Vector2.left; break;
            case DashDir.Right: dir = Vector2.right; break;
        }

        RaycastHit2D hit = Physics2D.Raycast(dashZonePos, dir, dashDistance, 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Floor"));
        float distance = !hit ? dashDistance : Vector3.Distance(hit.point, dashZonePos) - GetComponent<BoxCollider2D>().size.x / 2;
        m_Rigidbody2D.velocity = Vector3.zero;

        int dashCount = 10;
        Vector3 destination = (Vector3)dashZonePos + (Vector3)dir * distance - transform.position;
        for (int i = 0; i < dashCount; i++)
        {
            yield return null;
            transform.position += destination / dashCount;
        }
        m_Rigidbody2D.velocity = dir * 10;
        m_Controllable = true;
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}