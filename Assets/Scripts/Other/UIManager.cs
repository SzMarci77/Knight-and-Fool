using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI winScoreText;


    [Header("Highscore Flash Settings")]
    [SerializeField] private float flashSpeed = 0.3f;

    private int score = 0;
    private int highScore = 0;

    private bool isFlashing = false;
    private bool hasFlashedHI = false;
    private int flashCount = 0;

    [SerializeField] private Canvas gameCanvas;

    private void Awake()
    {
        if(Instance !=null  && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    //Temporary input for testing
    private void Update()
    {
        // HighScore reset
        if (Input.GetKeyDown(KeyCode.L))
        {
            ResetHighScore();
        }
    }

    private void OnEnable()
    {
        CharacterEvents.characterDamaged += (CharacterTookDamage);
        CharacterEvents.characterHealed += (CharacterHealed);
    }

    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= (CharacterTookDamage);
        CharacterEvents.characterHealed -= (CharacterHealed);
    }

    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        // Sebzés esetén szöveg létrehozása a karakter pozícióján
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position + Vector3.up * 1f);
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = damageReceived.ToString();
    }


    public void CharacterHealed(GameObject character, int healthRestored)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = healthRestored.ToString();
    }



    // ***** Score management *****
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScore(score);
    }

    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }

    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.SetInt("HighScore", highScore);
        UpdateUI();
    }

    public void UpdateScore(int newScore)
    {
        score = newScore;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);

            if (!hasFlashedHI)
            {
                StartCoroutine(BlinkHighScoreText());
                hasFlashedHI = true;
            }

        }
        UpdateUI();

        if (gameOverScoreText != null)
        {
            gameOverScoreText.text = "Score: " + score.ToString();
        }
        if (winScoreText != null)
        {
            winScoreText.text = "Score: " + score.ToString();
        }
    }

    private void UpdateUI()
    {
    if(scoreText != null)
        {
            scoreText.text = score.ToString("D4");
        }
    if(highScoreText != null && !isFlashing)
        {
            highScoreText.text = highScore.ToString("D4");
        }
    }

    // ***** HIGHSCORE FLASHING *****
    private IEnumerator BlinkHighScoreText()
    {
        isFlashing = true;
        flashCount = 0;

        while (flashCount < 6)
        {
            ColorUtility.TryParseHtmlString("#0DF461", out Color highlightColor);
            highScoreText.color = highlightColor;
            highScoreText.text = "New Record!";
            
            yield return new WaitForSeconds(flashSpeed);

            ColorUtility.TryParseHtmlString("#E98F45", out Color normallColor);
            highScoreText.color = normallColor;
            highScoreText.text = highScore.ToString("D4");

            yield return new WaitForSeconds(flashSpeed);

            flashCount++;
        }

        isFlashing = false;
        UpdateUI();
    }
}
