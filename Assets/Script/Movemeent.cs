using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float kecepatanPlayer = 3f;
    private Rigidbody2D rbPlayer;
    private InputAction gerakPlayer;

    // Animasi
    private Animator animasiPlayer;
    private SpriteRenderer spritePlayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();

        // Pastikan di Input Actions ada action bernama "Move" dengan type Vector2
        gerakPlayer = InputSystem.actions.FindAction("Move");

        animasiPlayer = GetComponent<Animator>();
        spritePlayer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerJalan();
    }

    void PlayerJalan()
    {
        Vector2 nilaiGerak = gerakPlayer.ReadValue<Vector2>();
        rbPlayer.linearVelocity = nilaiGerak * kecepatanPlayer;

        // --- Update animator parameters ---
        animasiPlayer.SetFloat("MoveX", nilaiGerak.x);
        animasiPlayer.SetFloat("MoveY", nilaiGerak.y);

        bool PlayerGerak = nilaiGerak.magnitude > 0.1f;
        animasiPlayer.SetBool("isWalking", PlayerGerak);

        // Balik sprite otomatis kalau ke kiri
        if (nilaiGerak.x != 0)
        {
            spritePlayer.flipX = nilaiGerak.x < 0;
            //spriteSapi.flipY = nilaiGerak.y < 0;
        }
    }
}
