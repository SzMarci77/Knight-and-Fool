using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private int highScore = 0;

    [SerializeField] private Canvas gameCanvas;

    private void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();
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
        // Create text at character healed
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = healthRestored.ToString();
    }

    public void UpdateScore(int score) 
    { 
        if(score > highScore) 
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            StartCoroutine(FlashHighScore());
        }
        scoreText.text = score.ToString("D4");
        highScoreText.text = highScore.ToString("D4");
    }

    private IEnumerator FlashHighScore()
    {
        Color originalColor = highScoreText.color;
        highScoreText.color = Color.green;
        yield return new WaitForSeconds(0.5f);
        highScoreText.color = originalColor;
    }
}
