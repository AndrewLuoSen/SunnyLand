using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private Transform checkGroundPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    private Rigidbody2D rigidbody2D;
    private Animator animator;

    private bool onGround;
    private bool faceRight = false;
    private float leftX;
    private float rightX;
    private static readonly int jump = Animator.StringToHash("Jump");
    private static readonly int fall = Animator.StringToHash("Fall");

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        leftX = left.position.x;
        rightX = right.position.x;
    }

    private void Update()
    {
        CheckOnGround();
        Debug.Log(onGround);
        Move();
        SwitchAnim();
    }

    private void CheckOnGround()
    {
        onGround = false;
        //检测是否在地面
        Collider2D[] colliders =
            Physics2D.OverlapBoxAll(checkGroundPoint.position, new Vector2(1, 0.2f), 0, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].transform != transform)
            {
                onGround = true;
                break;
            }
        }
    }

    private void Move()
    {
        if (faceRight)
        {
            if (onGround)
            {
                rigidbody2D.velocity = new Vector2(speed, jumpForce);

//                rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Force);
                animator.SetBool(jump, true);
            }
//            rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);

            if (transform.position.x >= rightX)
            {
                ChangeDir();
            }
        }
        else
        {
            if (onGround)
            {
                rigidbody2D.velocity = new Vector2(-speed, jumpForce);
//                rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Force);
                animator.SetBool(jump, true);
            }

            if (transform.position.x <= leftX)
            {
                ChangeDir();
            }
        }
    }

    private void ChangeDir()
    {
        faceRight = !faceRight;
        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
    }

    private void SwitchAnim()
    {
        if (animator.GetBool(jump))
        {
            if (rigidbody2D.velocity.y < 0)
            {
                animator.SetBool(jump, false);
                animator.SetBool(fall, true);
            }
        }

        if (onGround)
        {
            animator.SetBool(fall, false);
        }
    }
}