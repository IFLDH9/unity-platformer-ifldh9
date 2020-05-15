using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{

    public CharacterController2D controller;
    public Animator animator;

    [SyncVar]
    public float runSpeed = 40f;
    [SyncVar]
    float horizontalMove = 0f;
    [SyncVar]
    public bool jump = false;

    void Start()
    {

    }

    void Update()
    {
        if (isLocalPlayer)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            animator.SetFloat("speed", Mathf.Abs(horizontalMove));

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
                animator.SetBool("isJumping", true);
            }
        }
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            controller.CmdMove(horizontalMove * 0.02f, false, jump);
            jump = false;
        }
    }

    public void OnMining()
    {
        animator.SetBool("isMining", true);
    }

    public void OnStoppingMining()
    {
        animator.SetBool("isMining", false);
    }
}
