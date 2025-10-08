using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float deadZone = 0.1f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        float directionX = mouseWorldPos.x - transform.position.x;

        bool isMoving = Mathf.Abs(directionX) > deadZone;

        if (isMoving)
        {
            float moveDirection = Mathf.Sign(directionX);
            transform.position += new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0f, 0f);

            Vector3 scale = transform.localScale;
            scale.x = moveDirection * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        animator.SetBool("isMoving", isMoving);
    }
}
