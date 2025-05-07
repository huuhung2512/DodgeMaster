using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : SingletonBehavior<PlayerManager>
{
    public static bool gameOver;
    public static bool isGameStarted;
    public static bool isPaused;    // Biến để kiểm tra xem game có bị tạm dừng hay không
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject tabToStart;
    public GameObject settingPanel;
    public static int numberOfCoin;
    public static int playerScore; // Đổi thành int

    public Transform player;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI bestScoreText;  // Hiển thị best score
    public TextMeshProUGUI scoreText;      // Hiển thị điểm số khi kết thúc
    [SerializeField] private Animator scoreTextAnim;
    [SerializeField] private Animator bestScoreTextAnim;
    private float startingZ;    // Vị trí bắt đầu của player trên trục Z
    private int bestScore;      // Đổi bestScore thành int

    void Start()
    {
        ResetGameState();
        LoadBestScore();
        startingZ = player.position.z;  // Lưu lại vị trí Z ban đầu của player
    }

    void Update()
    {
        if (isPaused || gameOver) return;  // Nếu game đang bị tạm dừng hoặc game over thì không làm gì cả

        if (isGameStarted)
        {
            UpdatePlayerScore();
        }
        if (SwipeManager.tap && !isGameStarted)
        {
            StartGame();
        }
        // Cập nhật số coins
        UpdateCoinText();
    }

    void ResetGameState()
    {
        gameOver = false;
        isGameStarted = false;
        isPaused = false;  // Khởi tạo trạng thái không tạm dừng
        numberOfCoin = 0;
        playerScore = 0;
        gameOverPanel.SetActive(false);
        distanceText.gameObject.SetActive(true);
        coinText.gameObject.SetActive(true);
    }

    void LoadBestScore()
    {
        // Lấy best score đã lưu từ PlayerPrefs
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestScoreText.text = "Best Score:\n " + bestScore + " m";
    }

    public void HandleGameOver()
    {
        gameOver = true;
        gameOverPanel.SetActive(true);
        coinText.gameObject.SetActive(false);
        distanceText.gameObject.SetActive(false);
        SavePlayerData();
        // Hiển thị điểm số cuối cùng của người chơi
        scoreText.text = "Score:\n " + playerScore + " m";
        if (playerScore > bestScore)
        {
            scoreTextAnim.speed = 0;
            bestScoreTextAnim.speed = 1;
            Debug.Log("dang bat anim bestScore");
        }
        else
        {
            scoreTextAnim.speed = 1;
            bestScoreTextAnim.speed = 0;
            Debug.Log("0 bat anim bestScore");
        }
    }

    void UpdatePlayerScore()
    {
        playerScore = Mathf.FloorToInt(player.position.z - startingZ); // Dùng FloorToInt để lấy số nguyên
        distanceText.text = playerScore + " m";
    }

    void StartGame()
    {
        isGameStarted = true;
        tabToStart.SetActive(false);
    }

    void UpdateCoinText()
    {
        coinText.text = "Golds: " + numberOfCoin;
    }

    void SavePlayerData()
    {
        if (playerScore > bestScore)
        {
            bestScore = playerScore;
            bestScoreText.text = "Best Score: " + bestScore + " m";

            PlayerPrefs.SetInt("BestScore", bestScore);
        }
        int totalCoins = PlayerPrefs.GetInt("Coins", 0);
        totalCoins += numberOfCoin;
        PlayerPrefs.SetInt("Coins", totalCoins);
        PlayerPrefs.Save();
    }

    // Hàm để dừng game thủ công
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Dừng mọi hoạt động trong game
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Tiếp tục game
        pausePanel.SetActive(false);
    }
    public void ReloadGame()
    {
        SceneManager.LoadScene("Level 1");
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
        }
    }
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(GameEnum.ESound.ButtonClick);
        }
    }
}
