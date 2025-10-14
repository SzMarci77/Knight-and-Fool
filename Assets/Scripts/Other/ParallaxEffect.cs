using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{

    private float length, startpos;
    [SerializeField] public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        // Parallax mozgás
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // Végtelenített háttér
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
