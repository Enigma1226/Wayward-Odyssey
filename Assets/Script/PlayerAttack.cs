using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int damage = 25;
    [SerializeField] private float attackRange = 1.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    public void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            attackRange
        );

        foreach (Collider2D enemy in enemies)
        {
            Enemy target = enemy.GetComponent<Enemy>();

            if (target != null)
            {
                target.KenaDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}