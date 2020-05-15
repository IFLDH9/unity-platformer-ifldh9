using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class CharacterController2D : NetworkBehaviour
{
    [SerializeField] private float m_JumpForce = 4f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_CeilingCheck;

    const float k_GroundedRadius = .05f;
    [SyncVar]
    private bool m_Grounded;
    const float k_CeilingRadius = .2f;
    private Rigidbody2D m_Rigidbody2D;

    [SyncVar]
    private Vector3 playerTransformLocalScale;

    [SyncVar]
    private bool m_FacingRight = true;

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

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        transform.localScale = playerTransformLocalScale;
        player.nameText.transform.localScale = playerTransformLocalScale;
        bool wasGrounded = m_Grounded;
        m_Grounded = false;
        if (isLocalPlayer || isServer)
        {
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
    }

    [TargetRpc]
    private void TargetAddVelocity(NetworkConnection target, Vector3 targetVelocity)
    {
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }


    [TargetRpc]
    private void TargetAddForce(NetworkConnection target)
    {
        m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
        AudioHandler audio = GetComponent<AudioHandler>();
        audio.PlayJump();
        m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce), ForceMode2D.Impulse);
    }

    [Command]
    public void CmdMove(float move, bool crouch, bool jump)
    {
        if (m_Grounded || m_AirControl)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);

            TargetAddVelocity(connectionToClient, targetVelocity);

            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip();
            }
        }
        if (m_Grounded && jump)
        {
            m_Grounded = false;

            TargetAddForce(connectionToClient);
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = playerTransformLocalScale;
        theScale.x *= -1;
        playerTransformLocalScale = theScale;
        transform.localScale = playerTransformLocalScale;
        Player player = GetComponent<Player>();

    }
}