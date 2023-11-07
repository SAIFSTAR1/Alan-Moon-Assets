using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] private float attackRange, damage, power;
    [SerializeField] private LayerMask enemyLayer;

    public override void Attack(float dir)
    {
        var hit = Physics2D.OverlapCircle(transform.position, attackRange, enemyLayer);
        if (!hit) return;
        hit.GetComponent<Character>().Damage(damage);
        hit.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 0) * power, ForceMode2D.Impulse);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}