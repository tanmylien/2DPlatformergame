using UnityEngine;
using TMPro;   

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text scoreText;         
    public GameObject[] hearts;        

    [Header("Panels")]
    public GameObject startPanel;        
    public GameObject winPanel;
    public GameObject losePanel;

    public void UpdateScore(int cur, int target)
    {
        if (scoreText) scoreText.text = $"Cherries: {cur} / {target}";
    }

    public void UpdateHearts(int cur, int max)
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].SetActive(i < cur);
    }

    public void ShowStart()  { if (startPanel) startPanel.SetActive(true); }
    public void HideStart()  { if (startPanel) startPanel.SetActive(false); }

    public void ShowWin()
    {
        if (winPanel) winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowLose()
    {
        if (losePanel) losePanel.SetActive(true);
        Time.timeScale = 0f;
    }
}