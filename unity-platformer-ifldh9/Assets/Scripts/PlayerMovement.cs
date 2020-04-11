using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{

    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;

    [SyncVar]
    float horizontalMove = 0f;
    bool jump = false;

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

   public void OnLanding ()
    {
        animator.SetBool("isJumping", false);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime,false,jump);
        jump = false;
    }

}
