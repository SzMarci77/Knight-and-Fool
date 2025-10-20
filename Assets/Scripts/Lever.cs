using UnityEngine;

public class Lever : MonoBehaviour
{
    [Header("Lever Settings")]
    public GameObject secretWallTileMap;
    public KeyCode interactKey = KeyCode.Q;
    public GameObject interactText;
    public Animator leverAnimator;
    public AudioSource leverSound;

    private bool isActivated = false;
    private bool playerInRange = false;

    private void Start()
    {
        if (interactText != null)
            interactText.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (interactText != null && !interactText.activeSelf)
                interactText.SetActive(true);

            if (Input.GetKeyDown(interactKey))
            {
                isActivated = !isActivated;

                if (leverSound != null)
                    leverSound.Play();

                if (leverAnimator != null)
                    leverAnimator.SetBool("pull", isActivated);

                if (secretWallTileMap != null)
                    secretWallTileMap.SetActive(!isActivated);
            }
        }
        else if (interactText != null && interactText.activeSelf)
        {
            interactText.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactText != null)
                interactText.SetActive(false);
        }
    }
}
