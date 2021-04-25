using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerCharacter character;
    
    private float h;
    
    private void Start()
    {
        character = GetComponent<PlayerCharacter>();
    }
  
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        character.Move(h);

        if (Input.GetKeyDown(KeyCode.Space))
        {
             character.Jump();
        }
    }
}
