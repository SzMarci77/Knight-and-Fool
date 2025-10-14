using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public int damage = 10;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.Hit(damage, Vector2.zero);
            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
