using System;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int speed;
    [SerializeField] private int jumpForce;
    [SerializeField] private bool canAirControlMove;

    private int collectionCount;
    public int CollectionCount => collectionCount;

    private Animator animator;
    private Rigidbody2D rigidbody;

    private bool onGround;
    private bool faceRight = true;
    private bool isHurt;

    private static readonly int jump = Animator.StringToHash("Jump");
    private static readonly int fall = Animator.StringToHash("Fall");
    private static readonly int IsHurt = Animator.StringToHash("IsHurt");

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
            if (colliders[i].transform != transform)
            {
                onGround = true;
            }
        }

        SwitchAnim();
    }

    public void Move(float h)
    {
        if (isHurt)
        {
            return;
        }

        if (onGround || canAirControlMove)
        {
            if (h != 0)
            {
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
                animator.SetFloat("Velocity", Mathf.Abs(h));
            }
        }
        else
        {
            //对于摩擦力为0的，会自己动怎么办
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
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
            animator.SetBool(jump, true);
        }
    }

    private void SwitchAnim()
    {
        if (!onGround)
        {
            if (animator.GetBool(jump))
            {
                if (rigidbody.velocity.y < 0)
                {
                    animator.SetBool(jump, false);
                    animator.SetBool(fall, true);
                }
            }
            else if (rigidbody.velocity.y < 0)
            {
                animator.SetBool(fall, true);
            }
        }
        else
        {
            animator.SetBool(fall, false);
        }

        if (isHurt)
        {
            animator.SetBool(IsHurt, true);

            if (Mathf.Abs(rigidbody.velocity.x) < 1f)
            {
                isHurt = false;
                animator.SetBool(IsHurt, false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collection"))
        {
            collectionCount++;
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            //从空中下落碰撞 kill怪物
            if (animator.GetBool(fall) && !onGround)
            {
                Destroy(other.gameObject);
            }
            else
            {
                //从左边或者右边碰到则给一个反弹力 受伤
                if (transform.position.x < other.transform.position.x)
                {
                    //需要考虑什么时候重置 isHurt：当
                    //同时 isHurt要把move给禁用掉,不然反弹无效
                    isHurt = true;
                    rigidbody.velocity = new Vector2(-speed, rigidbody.velocity.y);
                }
                else
                {
                    isHurt = true;
                    rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
                }
            }
        }
    }

    public void AddItemCount()
    {
        
    }
}