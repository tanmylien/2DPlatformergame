using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("Goals & Health")]
    public int cherriesToWin = 10;
    public int maxHearts = 3;

    [Header("Scene References")]
    public Player player;
    public Transform spawnPoint;

    [Header("UI")]
    public UIManager ui;

    int score = 0;
    int hearts = 0;
    bool gameStarted = false;   

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
    }

    void Start()
    {
        hearts = maxHearts;
        ui?.UpdateScore(score, cherriesToWin);
        ui?.UpdateHearts(hearts, maxHearts);

        Time.timeScale = 0f;        
        ui?.ShowStart();           
        if (player) player.enabled = false; 
    }

    public void AddCherry(int amount = 1)
    {
        score += amount;
        ui?.UpdateScore(score, cherriesToWin);
        if (score >= cherriesToWin) Win();
    }

    public void AddHeart(int amount = 1)
    {
        hearts = Mathf.Clamp(hearts + amount, 0, maxHearts);
        ui?.UpdateHearts(hearts, maxHearts);
    }

    public void TakeDamage(int amount = 1)
    {
        hearts = Mathf.Max(0, hearts - amount);
        ui?.UpdateHearts(hearts, maxHearts);

        if (hearts <= 0) Lose();
    }

    public void KillPlayer()
    {
        hearts = 0;
        ui?.UpdateHearts(hearts, maxHearts);
        Lose();
    }

    void Win()
    {
        Time.timeScale = 0f;
        ui?.ShowWin();
    }

    void Lose()
    {
        Time.timeScale = 0f;
        ui?.ShowLose();
    }

    public void Respawn()
    {
        Time.timeScale = 1f;

        hearts = maxHearts;
        ui?.UpdateHearts(hearts, maxHearts);

        if (player != null && spawnPoint != null)
        {
            player.transform.SetParent(null);
            player.velocity = Vector2.zero;
            player.transform.position = spawnPoint.position;
        }
    }
    public void StartGame()                     
    {
        if (gameStarted) return;
        gameStarted = true;

        ui?.HideStart();
        if (player) player.enabled = true;
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void QuitToStart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start");
    }
}