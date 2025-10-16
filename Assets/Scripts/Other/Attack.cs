using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Collider2D attackCollider;

    [Header("Attack Settings")]
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;
    public float KBForce = 5f;

    private void Awake()
    {
        attackCollider = GetComponent<Collider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
    #if UNITY_EDITOR
        Debug.Log("Jatekos tamadas triggerelve! Cel: " + collision.gameObject.name);
    #endif

        Damageable damageable = collision.GetComponent<Damageable>();
        IDamageable damageable1 = collision.GetComponent<IDamageable>();

        if(damageable1 != null)
        {
            Vector2 deliveredKnockback1 = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            damageable1.Damagee(attackDamage, KBForce, deliveredKnockback1);
        }

        if (damageable != null && damageable.IsAlive)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);

            if (gotHit)
            {
        #if UNITY_EDITOR
                Debug.Log(collision.name + " megsebezve ennyivel: " + attackDamage);
        #endif
            }
        }
    }


}
