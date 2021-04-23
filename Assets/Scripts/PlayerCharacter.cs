using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int speed;
    [SerializeField] private int jumpForce;
    [SerializeField] private bool canAirControlMove;

    private Animator animator;
    private Rigidbody2D rigidbody;

    private bool onGround;
    private bool faceRight = true;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //射线检测是否在地面
        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheck.position, new Vector2(1, 0.1f), 0, groundLayer);

        onGround = false;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != gameObject)
            {
                onGround = true;
            }
        }

        if (rigidbody.velocity.y > 0)
        {
            animator.Play("JumpUp");
        }
        else
        {
            animator.Play("JumpDown");
        }
    }

    public void Move(float h)
    {
        if (onGround || canAirControlMove)
        {
            if (Mathf.Abs(h) > 0)
            {
                animator.Play("Run");
                //修改朝向
                if (faceRight && h < 0)
                {
                    ChangeDir();
                }
                else if (!faceRight && h > 0)
                {
                    ChangeDir();
                }

                //移动
                rigidbody.velocity = new Vector2(speed * h, rigidbody.velocity.y);
            }
            else
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                if (onGround)
                {
                    animator.Play("Idle");
                }
            }
        }
    }

    private void ChangeDir()
    {
        faceRight = !faceRight;
        transform.localScale = Vector2.Scale(transform.localScale, new Vector2(-1, 1));
    }

    public void Jump()
    {
        if (onGround)
        {
            onGround = false;
            Vector2 velocity = rigidbody.velocity;
            velocity.y = 0;
            rigidbody.velocity = velocity;
            rigidbody.AddForce(new Vector2(0, jumpForce));
        }
    }
}