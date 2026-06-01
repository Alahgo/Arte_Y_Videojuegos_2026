using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float speed = 5f;

    public Rigidbody2D rb;

    private Vector2 moveInput;

    void Update()
    {
     
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {

        rb.linearVelocity = new Vector2(moveInput.x * speed, moveInput.y * speed);
        
    }
}