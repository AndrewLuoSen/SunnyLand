using UnityEngine;

public class EnemyEagle : Enemy
{
    [SerializeField] protected Transform up;
    [SerializeField] protected Transform down;

    private bool flyUp = true;
    private float upY;
    private float downY;

    protected override void Init()
    {
        upY = up.position.y;
        downY = down.position.y;
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
        if (flyUp)
        {
            rigibody2d.velocity = new Vector2(0, speed);

            if (transform.position.y > upY)
            {
                flyUp = !flyUp;
            }
        }
        else
        {
            rigibody2d.velocity = new Vector2(0, -speed);

            if (transform.position.y < downY)
            {
                flyUp = !flyUp;
            }
        }
    }
}