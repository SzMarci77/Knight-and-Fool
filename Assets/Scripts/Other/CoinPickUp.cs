using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] private int coinValue = 50;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.AddScore(coinValue);
            }

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }
            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
