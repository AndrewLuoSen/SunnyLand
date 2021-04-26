using UnityEngine;

public class EnemyOpossum : Enemy
{
    [SerializeField] protected Transform left;
    [SerializeField] protected Transform right;

    private bool faceRight;
    private float leftX;
    private float rightX;

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

        Move();
    }

    protected override void Move()
    {
        if (faceRight)
        {
            rigibody2d.velocity = new Vector2(speed, 0);

            if (transform.position.x >= rightX)
            {
                ChangeDir();
            }
        }
        else
        {
            rigibody2d.velocity = new Vector2(-speed, 0);

            if (transform.position.x <= leftX)
            {
                ChangeDir();
            }
        }
    }

    protected override void ChangeDir()
    {
        faceRight = !faceRight;
        transform.localScale = Vector2.Scale(transform.localScale, new Vector2(-1, 1));
    }
}