using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] private GameObject deathSfx;

    protected Rigidbody2D rigibody2d;
    protected Animator anim;
    private CapsuleCollider2D collider2D;
    private SpriteRenderer sp;

    protected bool dead;

    void Start()
    {
        rigibody2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collider2D = GetComponent<CapsuleCollider2D>();
        sp = GetComponent<SpriteRenderer>();

        Init();
    }

    protected abstract void Init();

    protected abstract void Move();

    protected virtual void ChangeDir()
    {
    }

    protected virtual void SwitchAnim()
    {
    }

    public void OnDeath()
    {
        dead = true;
        collider2D.enabled = false;
        sp.enabled = false;
        deathSfx.SetActive(true);
        Invoke(nameof(DisableDeathSfx), 0.5f);
    }

    private void DisableDeathSfx()
    {
        deathSfx.SetActive(false);
    }
}