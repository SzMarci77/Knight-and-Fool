using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player halál kezelése
        if (collision.CompareTag("Player"))
        {
            Damageable damageable = collision.GetComponent<Damageable>();
            if (damageable != null && damageable.IsAlive)
            {
                // Instant halál
                damageable.Health = 0;
            }
        }
    }
}
