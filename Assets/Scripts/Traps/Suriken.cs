using UnityEngine;
using System.Collections;

public class Suriken : MonoBehaviour
{
    public Transform[] points;
    public int currentPoint = 0;
    public float speed = 5f;
    public int damage = 10;
    public float knockbackForce = 5f;

    private void Start()
    {
        if (points.Length > 0)
        {
            transform.position = points[0].position;
            currentPoint = 1; 
            StartCoroutine(MoveSuriken());
        }
    }

    private IEnumerator MoveSuriken()
    {
        while (true)
        {
            Transform targetPoint = points[currentPoint];

            while (Vector3.Distance(transform.position, targetPoint.position) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPoint.position,
                    speed * Time.deltaTime
                );
                yield return null;
            }
            currentPoint++;
            if (currentPoint >= points.Length)
                currentPoint = 0;

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null && damageable != this)
        {
            Vector2 knockDir = (other.transform.position - transform.position).normalized;

            damageable.Hit(damage, knockDir * knockbackForce);
        }
    }
}
