using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    [Header("Buttons Setup")]
    public GameObject levelButtons;
    public Sprite lockIcon;

    private Button[] buttons;

    private void Awake()
    {
        ButtonsToArray();
        SetupButtons();
    }

    private void ButtonsToArray()
    {
        int childCount = levelButtons.transform.childCount;
        buttons = new Button[childCount];
        for (int i = 0; i < childCount; i++)
        {
            buttons[i] = levelButtons.transform.GetChild(i).GetComponent<Button>();
        }
    }

    private void SetupButtons()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < buttons.Length; i++)
        {
            bool unlocked = i < unlockedLevel;
            Button btn = buttons[i];
            btn.interactable = unlocked;

            TMP_Text tmpText = btn.GetComponentInChildren<TMP_Text>();
            Transform existingLock = btn.transform.Find("LockIcon");
            Image lockImage = null;

            if (existingLock != null)
            {
                lockImage = existingLock.GetComponent<Image>();
            }
            else if (lockIcon != null)
            {
                GameObject lockObj = new GameObject("LockIcon", typeof(RectTransform), typeof(Image));
                lockObj.transform.SetParent(btn.transform, false);
                lockImage = lockObj.GetComponent<Image>();
                lockImage.sprite = lockIcon;

                RectTransform rt = lockObj.GetComponent<RectTransform>();
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                rt.pivot = new Vector2(0.5f, 0.5f);
                lockImage.preserveAspect = true;
            }

            if (!unlocked)
            {
                if (tmpText != null) tmpText.enabled = false;
                if (lockImage != null) lockImage.enabled = true;
            }
            else
            {
                if (tmpText != null) tmpText.enabled = true;
                if (lockImage != null) lockImage.enabled = false;
            }
        }
    }

    private void Update()
    {
        // Debug: törlés K gombbal
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerPrefs.DeleteKey("UnlockedLevel");
            PlayerPrefs.Save();
            Debug.Log("Progress Reset");
        }
    }
}
