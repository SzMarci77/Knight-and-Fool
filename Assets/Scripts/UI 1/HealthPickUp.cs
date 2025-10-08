using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int healthAmount = 20;
    public Vector3 spinRotationSpeed = new Vector3(0, 100, 0);

    AudioSource pickUpSource;

    private void Awake()
    {
        pickUpSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        //Forgás effektus
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable && damageable.Health < damageable.MaxHealth)
        {
            bool waxHealed = damageable.Heal(healthAmount);

            if (waxHealed)
            {
                // Hang effekt
                if (pickUpSource)
                {
                    AudioSource.PlayClipAtPoint(pickUpSource.clip, gameObject.transform.position, pickUpSource.volume);
                }
                Destroy(gameObject);
            }
            
        }
    }


}
