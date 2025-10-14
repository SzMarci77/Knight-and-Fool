using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField]private float lineOfSite;
    [SerializeField]private float shootingRange;
    [SerializeField]private float fireRate = 1f;


    public GameObject bullet;
    public GameObject bulletParent;

    private float nextFireTime;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if(distanceFromPlayer < lineOfSite && distanceFromPlayer > shootingRange)
        {
        transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }
        else if(distanceFromPlayer <= shootingRange && nextFireTime < Time.time)
        {
            Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
