using UnityEngine;
using UnityEngine.Serialization;

public class EnemyFrog : Enemy
{
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private float jumpForce;
    [FormerlySerializedAs("groundCheck")] [SerializeField] protected Transform groundCheckPoint;
    [SerializeField] protected LayerMask groundLayer;

    private bool changeSpeedDir = true;
    private bool faceRight;
    private float leftX;
    private float rightX;
    private bool onGround;

    private static readonly int jump = Animator.StringToHash("Jump");
    private static readonly int fall = Animator.StringToHash("Fall");
    private static readonly int idle = Animator.StringToHash("Idle");

    protected override void Init()
    {
        leftX = left.position.x;
        rightX = right.position.x;
    }

    private void Update()
    {
        if (dead)
        {
            return;
        }
        
        CheckIsOnGround();
        Move();
    }

    private void CheckIsOnGround()
    {
        Collider2D[] colliders =
            Physics2D.OverlapBoxAll(groundCheckPoint.position, new Vector2(1, 0.2f), 0, groundLayer);

        onGround = false;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != gameObject)
            {
                onGround = true;
            }
        }

        SwitchAnim();
    }


    protected override void Move()
    {
        if (faceRight)
        {
            if (!changeSpeedDir)
            {
                rigibody2d.velocity = new Vector2(speed, rigibody2d.velocity.y);
                changeSpeedDir = true;
            }

            if (onGround)
            {
                if (CheckAnimIsFinish() && CanJUmp())
                {
                    anim.SetBool(jump, true);
                    rigibody2d.velocity = new Vector2(speed, jumpForce);
                }

                if (transform.position.x >= rightX)
                {
                    ChangeDir();
                }
            }
        }
        else
        {
            if (!changeSpeedDir)
            {
                rigibody2d.velocity = new Vector2(-speed, rigibody2d.velocity.y);
                changeSpeedDir = true;
            }

            if (onGround)
            {
                if (CheckAnimIsFinish() && CanJUmp())
                {
                    anim.SetBool(jump, true);
                    rigibody2d.velocity = new Vector2(-speed, jumpForce);
                }

                if (transform.position.x <= leftX)
                {
                    ChangeDir();
                }
            }
        }
    }


    protected override void ChangeDir()
    {
        faceRight = !faceRight;
        transform.localScale = Vector2.Scale(transform.localScale, new Vector2(-1, 1));
        changeSpeedDir = false;
    }

    protected override void SwitchAnim()
    {
        if (!onGround)
        {
            if (anim.GetBool(jump))
            {
                if (rigibody2d.velocity.y < 0)
                {
                    anim.SetBool(jump, false);
                    anim.SetBool(fall, true);
                }
            }
        }
        else
        {
            anim.SetBool(fall, false);
            anim.SetBool(idle, true);
        }
    }

    private bool CheckAnimIsFinish()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f)
        {
            return true;
        }

        return false;
    }

    private bool CanJUmp()
    {
        int type = Random.Range(0, 2);
        return type == 0 && !anim.GetBool(fall);
    }
}