using UnityEngine;

public class Item : MonoBehaviour
{
    private SpriteRenderer sp;
    private BoxCollider2D collider;
    private GameObject feedbackSfx;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        feedbackSfx = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCharacter>().AddItemCount();
            sp.enabled = false;
            collider.enabled = false;
            feedbackSfx.SetActive(true);
            Invoke(nameof(DisableFeedbackSfx), 0.5f);//记得改时间
        }
    }

    private void DisableFeedbackSfx()
    {
        feedbackSfx.SetActive(false);
    }
}