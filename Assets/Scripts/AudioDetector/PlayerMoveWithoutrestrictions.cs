using UnityEngine;

public class PlayerMoveWithoutrestrictions : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;

    [Header("Configuración de Lag Retró")]
    [Tooltip("Cuánto tiempo (en segundos) tarda el personaje en detenerse tras soltar las teclas")]
    public float inputLagDuration = 0.12f;
    private float lagTimer;

    private Vector2 moveInput;
    private Animator animator;
    private bool is_Moving;

    private void Start()
    {
        animator = GetComponent<Animator>();
        is_Moving = false;
    }

    void Update()
    {

        float rawX = Input.GetAxisRaw("Horizontal");
        float rawY = Input.GetAxisRaw("Vertical");


        if (rawX != 0) rawY = 0;


        if (rawX != 0 || rawY != 0)
        {

            moveInput.x = rawX;
            moveInput.y = rawY;
            is_Moving = true;

            lagTimer = inputLagDuration;
        }
        else
        {

            if (lagTimer > 0)
            {
                lagTimer -= Time.deltaTime;

            }
            else
            {

                moveInput = Vector2.zero;
                is_Moving = false;
            }
        }

        if (is_Moving)
        {
            animator.SetFloat("Horizontal", moveInput.x);
            animator.SetFloat("Vertical", moveInput.y);
        }

        animator.SetBool("Is_Moving", is_Moving);
    }

    void FixedUpdate()
    {
    
        rb.linearVelocity = moveInput * speed;
        
    }
}
