using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int score;
    private int highScore;

    private float scoreMultiplier = 1.0f;  // スコア倍率（初期値は1.0）

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScore();  // ゲーム開始時に最高スコアをロード
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points)
    {
        // 倍率を適用してスコアを加算
        score += Mathf.RoundToInt(points * scoreMultiplier);
    }

    public void IncreaseMultiplier(float amount)
    {
        scoreMultiplier += amount;  // 倍率を増加
    }

    public void ResetMultiplier()
    {
        scoreMultiplier = 1.0f;  // 倍率をリセット
    }

    public int GetScore()
    {
        return score;
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void SaveHighScore()
    {
        if (score > highScore)
        {
            highScore = score;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);  // デフォルトは0
    }

    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();
    }
}
