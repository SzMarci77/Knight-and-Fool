using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 2f;
    [SerializeField] public GameObject[] wayPoints;

    private int nextWaypoint = 1;
    private float distToPoint;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        distToPoint = Vector2.Distance(transform.position, wayPoints[nextWaypoint].transform.position);
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[nextWaypoint].transform.position, moveSpeed * Time.deltaTime);
        transform.localScale = new Vector3(-1, 1, 1);
        if (distToPoint < 0.2f)
        {
            TakeTurn();
        }
    }

    private void TakeTurn()
    {
        Vector3 currRot = transform.eulerAngles;
        currRot.z += wayPoints[nextWaypoint].transform.eulerAngles.z;
        transform.eulerAngles = currRot;
        ChooseNextWaypoint();
    }

    private void ChooseNextWaypoint()
    {
        nextWaypoint++;
        if (nextWaypoint == wayPoints.Length)
        {
            nextWaypoint = 0;
        }
    }
}
