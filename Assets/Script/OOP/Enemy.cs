using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageable
{
    public static event Action<Enemy> OnSkeletonMati;
    [SerializeField] private UnityEvent onSkeletonMati;
    [SerializeField] private SkeletonConfig config;
    private int hp;
    public float ms;
    protected Transform player;

    [Header("Pengaturan State Machine")]
    private float jarakDeteksi;
    private float jarakSerang;
    [Header("Pengaturan Serangan")]
    [SerializeField] private int damageSerang = 10;
    [SerializeField] private float jedaSerang = 1f;

    [Header("Pengaturan Patrol A → B")]
    [SerializeField] private Transform titikA;
    [SerializeField] private Transform titikB;
    [SerializeField] private float msPatrol = 1.5f;

    // State
    private StateSkeleton state = StateSkeleton.IDLE;
    private float waktuSerangTerakhir;

    // Patrol
    private Transform targetPatrol;

    void Awake()
    {
        if (config != null)
        {
            hp = config.hp;
            ms = config.kecepatan;
            jarakDeteksi = config.jarakDeteksi;
            jarakSerang = config.jarakSerang;
        }
        else
        {
            Debug.LogWarning("Config Skeleton belum di-assign!");
        }
    }

    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        if (titikA != null)
            targetPatrol = titikA;
    }

    void Update()
    {
        PeriksaTransisi();

        switch (state)
        {
            case StateSkeleton.IDLE:   PerilakuIdle();   break;
            case StateSkeleton.PATROL: PerilakuPatrol(); break;
            case StateSkeleton.CHASE:  PerilakuChase();  break;
            case StateSkeleton.ATTACK: PerilakuAttack(); break;
        }
    }

    public float JarakKePlayer()
    {
        if (player == null) return Mathf.Infinity;
        return Vector2.Distance(transform.position, player.position);
    }

    void PeriksaTransisi()
    {
        float jarak = JarakKePlayer();

        if (jarak <= jarakSerang)
            state = StateSkeleton.ATTACK;
        else if (jarak <= jarakDeteksi)
            state = StateSkeleton.CHASE;
        else
        {
            if (titikA != null && titikB != null)
                state = StateSkeleton.PATROL;
            else
                state = StateSkeleton.IDLE;
        }
    }

    void PerilakuIdle() { }

    void PerilakuPatrol()
    {
        if (titikA == null || titikB == null)
        {
            Debug.LogWarning("TitikA atau TitikB belum di-assign!");
            return;
        }

        if (targetPatrol == null)
        {
            Debug.LogWarning("targetPatrol null!");
            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPatrol.position,
            msPatrol * Time.deltaTime
        );

        FlipSprite(targetPatrol.position);

        float jarak = Vector2.Distance(transform.position, targetPatrol.position);
        if (jarak <= 0.1f)
            targetPatrol = (targetPatrol == titikA) ? titikB : titikA;
    }

    void PerilakuChase()
    {
        Kejar();
    }

    void PerilakuAttack()
    {
        if (Time.time - waktuSerangTerakhir >= jedaSerang)
        {
            Serang();
            waktuSerangTerakhir = Time.time;
        }
    }

    void FlipSprite(Vector3 targetPos)
    {
        if (targetPos.x < transform.position.x)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    public void Kejar()
    {
        if (player == null) return;
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            ms * Time.deltaTime
        );
        FlipSprite(player.position);
    }

    public virtual void Serang()
    {
        Debug.Log($"{gameObject.name} menyerang!");
        if (player != null)
        {
            IDamageable target = player.GetComponent<IDamageable>();
            if (target != null)
            {
                target.KenaDamage(damageSerang);
            }
        }
    }

    public void KenaDamage(int jumlah)
    {
        hp -= jumlah;
        Debug.Log($"{gameObject.name} kena damage {jumlah}, HP sisa: {hp}");
        if (hp <= 0) Mati();
    }

    private void Mati()
    {
        Debug.Log($"{gameObject.name} mati!");
        OnSkeletonMati?.Invoke(this);
        onSkeletonMati?.Invoke();
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (titikA != null && titikB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(titikA.position, titikB.position);
            Gizmos.DrawWireSphere(titikA.position, 0.2f);
            Gizmos.DrawWireSphere(titikB.position, 0.2f);
            UnityEditor.Handles.Label(titikA.position + Vector3.up * 0.3f, "A");
            UnityEditor.Handles.Label(titikB.position + Vector3.up * 0.3f, "B");
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, jarakDeteksi);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, jarakSerang);
    }
}
