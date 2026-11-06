using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    public float kecepatanPlayer = 3f;
    private Rigidbody2D rbPlayer;
    private InputAction gerakPlayer;

    // Animasi
    private Animator animasiPlayer;
    private SpriteRenderer spritePlayer;

    // HP Player
    [Header("HP Player")]
    [SerializeField] private int maxHP = 100;
    [SerializeField] private HealthBarUI healthBar;
    private int hp;

    // Respawn
    private Vector3 posisiAwal;
    private bool sudahMati = false;
    private Collider2D col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();

        // Pastikan di Input Actions ada action bernama "Move" dengan type Vector2
        gerakPlayer = InputSystem.actions.FindAction("Move");

        animasiPlayer = GetComponent<Animator>();
        spritePlayer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        hp = maxHP;
        posisiAwal = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (sudahMati)
        {
            // Pencet Space untuk respawn
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Respawn();
            }
            return;
        }

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

    public void KenaDamage(int jumlah)
    {
        if (sudahMati) return;

        hp -= jumlah;
        Debug.Log($"Player kena damage {jumlah}, HP sisa: {hp}");
        if (healthBar != null) healthBar.SetHealth(hp);
        if (hp <= 0) PlayerMati();
    }

    private void PlayerMati()
    {
        Debug.Log("Game Over! Tekan Space untuk respawn.");
        sudahMati = true;

        // Player hilang (sembunyiin sprite, matiin physics)
        spritePlayer.enabled = false;
        rbPlayer.linearVelocity = Vector2.zero;
        rbPlayer.simulated = false;
        if (col != null) col.enabled = false;
    }

    private void Respawn()
    {
        Debug.Log("Player respawn!");
        sudahMati = false;

        // Reset HP
        hp = maxHP;
        if (healthBar != null) healthBar.SetHealth(hp);

        // Balik ke posisi awal
        transform.position = posisiAwal;

        // Player muncul lagi
        spritePlayer.enabled = true;
        rbPlayer.simulated = true;
        if (col != null) col.enabled = true;
    }
}
