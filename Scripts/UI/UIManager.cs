using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PixelGunner.Managers;

namespace PixelGunner.UI
{
    /// <summary>
    /// Quản lý toàn bộ giao diện người dùng (HUD Canvas, Game Over, Victory, Thanh máu, Điểm số).
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("--- THANH MÁU & CHỈ SỐ PLAYER ---")]
        [SerializeField] private Slider healthSlider;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI scoreText;

        [Header("--- CÁC BẢNG ĐIỀU KHIỂN (PANELS) ---")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private GameObject pausePanel;

        private int cachedPlayerHp = 100;
        private int cachedMaxHp = 100;
        private int cachedScore = 0;
        private int cachedCoins = 0;

        private bool isGameOver = false;
        private bool isVictory = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (victoryPanel != null) victoryPanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(false);
        }

        public void UpdatePlayerHealth(int currentHealth, int maxHealth)
        {
            cachedPlayerHp = currentHealth;
            cachedMaxHp = maxHealth;

            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
            }

            if (healthText != null)
            {
                healthText.text = $"{currentHealth} / {maxHealth}";
            }
        }

        public void UpdateCoinCount(int coins)
        {
            cachedCoins = coins;
            if (coinText != null) coinText.text = $"Coins: {coins}";
        }

        public void UpdateScore(int score)
        {
            cachedScore = score;
            if (scoreText != null) scoreText.text = $"Score: {score}";
        }

        public void ShowGameOver()
        {
            isGameOver = true;
            isVictory = false;
            Time.timeScale = 0f;
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
        }

        public void ShowVictory()
        {
            if (isGameOver) return;
            isVictory = true;
            Time.timeScale = 0f;
            if (victoryPanel != null) victoryPanel.SetActive(true);
        }

        public void TogglePauseMenu(bool isPaused)
        {
            if (pausePanel != null) pausePanel.SetActive(isPaused);
        }

        private void OnGUI()
        {
            // 1. GÓC TRÊN BÊN TRÁI: HUD BẢNG MÁU, ĐIỂM SỐ & QUÁI VẬT
            GUI.Box(new Rect(15, 15, 250, 105), GUIContent.none);

            GUIStyle labelBold = new GUIStyle(GUI.skin.label);
            labelBold.fontStyle = FontStyle.Bold;
            labelBold.normal.textColor = Color.white;

            GUI.Label(new Rect(25, 20, 230, 20), $"HP: {cachedPlayerHp} / {cachedMaxHp}", labelBold);

            // Vẽ thanh máu
            float hpPercent = cachedMaxHp > 0 ? Mathf.Clamp01((float)cachedPlayerHp / cachedMaxHp) : 0;
            Color oldCol = GUI.color;
            GUI.color = Color.black;
            GUI.DrawTexture(new Rect(25, 42, 230, 14), Texture2D.whiteTexture);
            GUI.color = hpPercent > 0.3f ? new Color(0.2f, 0.85f, 0.3f) : new Color(0.95f, 0.2f, 0.2f);
            GUI.DrawTexture(new Rect(26, 43, 228 * hpPercent, 12), Texture2D.whiteTexture);
            GUI.color = oldCol;

            // Đếm quái vật còn lại
            int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            GUI.Label(new Rect(25, 62, 230, 18), $"Điểm: {cachedScore}   |   Vàng: {cachedCoins}", labelBold);
            GUI.Label(new Rect(25, 82, 230, 18), $"Quái vật còn lại: {enemyCount} con", labelBold);

            // 2. BẢNG CHIẾN THẮNG HOÀN THÀNH MÀN CHƠI (VICTORY SCREEN)
            if (isVictory)
            {
                GUI.color = new Color(0, 0, 0, 0.85f);
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
                GUI.color = Color.white;

                float pw = 460;
                float ph = 270;
                float px = (Screen.width - pw) / 2f;
                float py = (Screen.height - ph) / 2f;

                GUI.Box(new Rect(px, py, pw, ph), GUIContent.none);

                GUIStyle winTitle = new GUIStyle(GUI.skin.label);
                winTitle.fontSize = 28;
                winTitle.fontStyle = FontStyle.Bold;
                winTitle.alignment = TextAnchor.MiddleCenter;
                winTitle.normal.textColor = new Color(1f, 0.88f, 0.15f); // Vàng Gold

                GUI.Label(new Rect(px, py + 20, pw, 42), "★ CHIẾN THẮNG! ★", winTitle);

                GUIStyle subText = new GUIStyle(GUI.skin.label);
                subText.fontSize = 15;
                subText.alignment = TextAnchor.MiddleCenter;
                subText.normal.textColor = Color.white;

                GUI.Label(new Rect(px, py + 70, pw, 24), "Bạn đã quét sạch quái vật và hoàn thành màn chơi!", subText);
                GUI.Label(new Rect(px, py + 98, pw, 24), $"Tổng Điểm: {cachedScore}   |   Vàng: {cachedCoins}", subText);

                GUIStyle btnStyle = new GUIStyle(GUI.skin.button);
                btnStyle.fontSize = 17;
                btnStyle.fontStyle = FontStyle.Bold;

                if (GUI.Button(new Rect(px + 80, py + 145, pw - 160, 50), "CHƠI LẠI (PLAY AGAIN)", btnStyle))
                {
                    isVictory = false;
                    Time.timeScale = 1f;
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.RestartGame();
                    }
                }

                if (GUI.Button(new Rect(px + 130, py + 210, pw - 260, 32), "Thoát Game"))
                {
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.QuitGame();
                    }
                }
            }

            // 3. BẢNG GAME OVER KHI HẾT MÁU
            if (isGameOver)
            {
                GUI.color = new Color(0.18f, 0, 0, 0.88f);
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
                GUI.color = Color.white;

                float pw = 420;
                float ph = 240;
                float px = (Screen.width - pw) / 2f;
                float py = (Screen.height - ph) / 2f;

                GUI.Box(new Rect(px, py, pw, ph), GUIContent.none);

                GUIStyle overTitle = new GUIStyle(GUI.skin.label);
                overTitle.fontSize = 26;
                overTitle.fontStyle = FontStyle.Bold;
                overTitle.alignment = TextAnchor.MiddleCenter;
                overTitle.normal.textColor = new Color(1f, 0.25f, 0.25f);

                GUI.Label(new Rect(px, py + 20, pw, 40), "☠ BẠN ĐÃ BỊ HẠ GỤC ☠", overTitle);

                GUIStyle overSub = new GUIStyle(GUI.skin.label);
                overSub.fontSize = 15;
                overSub.alignment = TextAnchor.MiddleCenter;
                overSub.normal.textColor = Color.white;

                GUI.Label(new Rect(px, py + 70, pw, 24), "Bạn đã bị quái vật đánh bại!", overSub);
                GUI.Label(new Rect(px, py + 95, pw, 24), $"Điểm số: {cachedScore}   |   Vàng: {cachedCoins}", overSub);

                GUIStyle btnStyle = new GUIStyle(GUI.skin.button);
                btnStyle.fontSize = 16;
                btnStyle.fontStyle = FontStyle.Bold;

                if (GUI.Button(new Rect(px + 70, py + 140, pw - 140, 48), "THỬ LẠI (RETRY)", btnStyle))
                {
                    isGameOver = false;
                    Time.timeScale = 1f;
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.RestartGame();
                    }
                }
            }
        }
    }
}
