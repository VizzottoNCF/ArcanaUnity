using System.Globalization;
using UnityEngine;

public class TopDownController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Animator animatorBody;
    private Animator animatorCape;
    private Animator animatorHat;

    [SerializeField] float speed;
    private Vector2 moveInput;
    private enum re_FacingDirection { UP, DOWN, RIGHT, LEFT }
    private re_FacingDirection faceDir;

    private void Start()
    {
        animator = transform.Find("Sprites").GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        faceDir = re_FacingDirection.DOWN;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q)) { GetComponent<RippleVFX>().rf_SpawnRipple(transform); }

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // normalizes diagonals 
        moveInput.Normalize();

        rf_UpdateFacingDirection();

        rf_UpdateAnimation();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = speed * moveInput;
    }

    private void rf_UpdateFacingDirection()
    {
        if (moveInput != Vector2.zero)
        {
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                faceDir = moveInput.x > 0 ? re_FacingDirection.RIGHT : re_FacingDirection.LEFT;
            }
            else
            {
                faceDir = moveInput.y > 0 ? re_FacingDirection.UP : re_FacingDirection.DOWN;
            }
        }
    }

    private void rf_UpdateAnimation()
    {
        if (rb.linearVelocity.x == 0 && rb.linearVelocity.y == 0)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }

        if (faceDir == re_FacingDirection.DOWN) { animator.SetInteger("Direction", 1); }
        else if (faceDir == re_FacingDirection.LEFT) { animator.SetInteger("Direction", 2); }
        else if (faceDir == re_FacingDirection.UP) { animator.SetInteger("Direction", 3); }
        else if (faceDir == re_FacingDirection.RIGHT) { animator.SetInteger("Direction", 4); }
    }
}