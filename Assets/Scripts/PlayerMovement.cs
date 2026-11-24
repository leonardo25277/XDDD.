using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f;
    public Joystick joystick;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movimiento;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movimiento = joystick.Direction;

        animator.SetFloat("MoveX", movimiento.x);
        animator.SetFloat("MoveY", movimiento.y);
    }

    void FixedUpdate()
    {
        rb.MovePosition(
            rb.position + movimiento * velocidad * Time.fixedDeltaTime
        );
    }
}