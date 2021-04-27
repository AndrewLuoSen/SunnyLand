using UnityEngine;

public class EnemyIdleMove : EnemyFrog
{
    protected int dir = -1;

    protected override void Init()
    {
        rigibody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }
    
    protected override void Move()
    {
        if (CheckForwardIsGround())
        {
            rigibody2d.velocity = new Vector2(speed * dir, 0);
        }
    }

    protected bool CheckForwardIsGround()
    {
        var hit = Physics2D.Raycast(groundCheckPoint.position + transform.right * dir, Vector2.down, 1f, groundLayer);
        if (hit.collider == null)
        {
            dir *= -1;
            ChangeDir();
            return false;
        }

        return true;
    }
}