using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;


public class CharacterController2D : NetworkBehaviour
{
    [SerializeField] private float m_JumpForce = 4f;                          // Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings

    const float k_GroundedRadius = .05f; // Radius of the overlap circle to determine if grounded
    [SyncVar]
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;

    [SyncVar]
    private Vector3 playerTransformLocalScale;

    [SyncVar]
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    [SyncVar]
    private Vector3 m_Velocity = Vector3.zero;
    
    Player player;
    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

   

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        player = GetComponent<Player>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        playerTransformLocalScale = transform.localScale;

        Debug.Log("ennyi lett a scale: " + playerTransformLocalScale);

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        transform.localScale = playerTransformLocalScale;
        player.nameText.transform.localScale = playerTransformLocalScale;
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
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    [TargetRpc]
    public void TargetAddVelocity(NetworkConnection target, Vector3 targetVelocity)
    {
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }


    [TargetRpc]
    public void TargetAddForce(NetworkConnection target)
    {
        m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce),ForceMode2D.Impulse);
       // jump = false;
    }

    [Command]
    public void CmdMove(float move, bool crouch, bool jump)
    {
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character

            TargetAddVelocity(connectionToClient, targetVelocity);
           // m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                  //  Flip();
                Flip();
               // TargetNameTextChange(connectionToClient, transform.localScale);
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                 //  Flip();
                Flip();
                //TargetNameTextChange(connectionToClient, transform.localScale);
            }
        }
        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            TargetAddForce(connectionToClient);
          // m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
          //  jump = false;
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = playerTransformLocalScale;
        theScale.x *= -1;
        playerTransformLocalScale = theScale;
        transform.localScale = playerTransformLocalScale;
        Player player = GetComponent<Player>();
        

       // player.nameText.transform.localScale = theScale;
                
        
       // player.nameText.transform.localScale = new Vector3(-1,1,1);
        //  RpcFlip(theScale);
    }

    [TargetRpc]
    public void TargetNameTextChange(NetworkConnection target, Vector3 localScale)
    {
        Player player = GetComponent<Player>();
        player.nameText.transform.localScale = localScale;
    }

    [Command]
    private void CmdFlip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = playerTransformLocalScale;
        theScale.x *= -1;
        playerTransformLocalScale = theScale;
        transform.localScale = playerTransformLocalScale;
       // Debug.Log(m_FacingRight);

        // RpcFlip(m_FacingRight);
    }


    [ClientRpc]
    private void RpcFlip(bool m_FacingRight)
    {
      //  if(this.m_FacingRight != m_FacingRight)
        {
               m_FacingRight = !m_FacingRight;
             Vector3 theScale = transform.localScale;
             theScale.x *= -1;
            transform.localScale = theScale;
        
        }
 // Switch the way the player is labelled as facing.
         
        // Multiply the player's x local scale by -1.


     
    }

}