using UnityEngine;

//斜面运动
public class EnemyInclinedPlaneMove : EnemyIdleMove
{
    private Vector2 moveDirX;

    void Update()
    {
        Move();
    }

    protected override void Move()
    {
        if (CheckForwardIsGround())
        {
            RaycastHit2D hitInfo = GetHitInfo();
            //将正上方设置为斜面的法向量
            transform.up = hitInfo.normal;
            if (hitInfo.normal == Vector2.up)
            {
                rigibody2d.velocity = new Vector2(speed * dir, 0);
            }
            else
            {
                float angle = Vector3.Angle(hitInfo.normal, Vector3.right * -1);
                angle = 90 - angle;
                rigibody2d.velocity = new Vector2(speed * Mathf.Cos(angle * Mathf.Deg2Rad) * dir,
                    speed * Mathf.Sin(angle * Mathf.Deg2Rad) * dir);
                Debug.DrawLine(transform.position, new Vector3(rigibody2d.velocity.x, rigibody2d.velocity.y,0) + transform.position, Color.red);
            }
        }
    }

    private RaycastHit2D GetHitInfo()
    {
        return Physics2D.Raycast(groundCheckPoint.position, -transform.up, 1, groundLayer);
    }
}