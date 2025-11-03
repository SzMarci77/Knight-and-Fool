using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [Header("Spike Trap Settings")]
    public int instantKillDamage = 9999;
    public Vector2 knockbackForce = new Vector2(0, -1f);
    public AudioSource spikeSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Damageable damageable = collision.GetComponent<Damageable>();

            if (damageable != null && damageable.IsAlive)
            {
                if (spikeSound != null)
                    spikeSound.Play();

                damageable.Hit(instantKillDamage, knockbackForce);
            }
        }

    }
}