using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] private int healthAmount = 20;
    [SerializeField] private Vector3 spinRotationSpeed = new Vector3(0, 100, 0);
    [SerializeField] private GameObject pickupEffect;

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
            bool maxHealed = damageable.Heal(healthAmount);

            if (maxHealed)
            {
                // Hang effekt
                if (pickUpSource)
                {
                    AudioSource.PlayClipAtPoint(pickUpSource.clip, gameObject.transform.position, pickUpSource.volume);
                }
                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
            
        }
    }


}
